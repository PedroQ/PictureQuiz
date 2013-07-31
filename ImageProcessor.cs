using Nokia.Graphics.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Nokia.Graphics;
using Nokia.InteropServices.WindowsRuntime;
using System.Windows;

namespace PictureQuiz
{
    public class ImageProcessor
    {
        public enum Difficulty
        {
            Easy = 1,
            Moderate,
            Hard,
            VeryHard,
            ReallyHard
        }

        // Filter groups for each difficulty
        private FilterGroup _easyFilterGroup;
        private FilterGroup _moderateFilterGroup;
        private FilterGroup _hardFilterGroup;
        private FilterGroup _veryHardFilterGroup;
        private FilterGroup _impossibleFilterGroup;

        // Masks to be used with the ImageFusion Filter
        private Bitmap _mask1;
        private Bitmap _mask2;
        private Bitmap _mask3;
        private Bitmap _maskBackground;

        //Our editing sessions
        EditingSession _session;

        public ImageProcessor()
        {
            //Initialize the filter groups for each difficulty level
            SetupDifficultyFilterGroups();
        }

        private void SetupDifficultyFilterGroups()
        {
            // Alpha Masks
            _mask1 = LoadBitmap("mask1.jpg");
            _mask2 = LoadBitmap("mask2.jpg");
            _mask3 = LoadBitmap("mask3.jpg");

            // Alpha mask background
            _maskBackground = LoadBitmap("maskBg.jpg");


            // Easy Difficulty: Antique filter and 23 degree rotation
            _easyFilterGroup = new FilterGroup(new IFilter[]
            {
                FilterFactory.CreateAntiqueFilter(),
                FilterFactory.CreateFreeRotationFilter(23f, RotationResizeMode.FitInside)
            });


            // Moderate Difficulty: Milky and Blur filters and alpha mask 1
            _moderateFilterGroup = new FilterGroup(new IFilter[]
            {
                FilterFactory.CreateMilkyFilter(),
                FilterFactory.CreateBlurFilter(BlurLevel.Blur3),
                FilterFactory.CreateImageFusionFilter(_maskBackground, _mask1, false)
            });

            // Hard Difficulty: Warp, Magic Pen and Gray Scale Negative filters
            _hardFilterGroup = new FilterGroup(new IFilter[]
            {
                FilterFactory.CreateWarpFilter(WarpEffect.SmallNose, 0.8f),
                FilterFactory.CreateMagicPenFilter(),
                FilterFactory.CreateGrayscaleNegativeFilter()
            });

            // Very Hard Difficulty: Cartoon filter and apply alpha mask 2
            _veryHardFilterGroup = new FilterGroup(new IFilter[]
            {
                FilterFactory.CreateCartoonFilter(true),
                FilterFactory.CreateImageFusionFilter(_maskBackground, _mask2, false)
            });

            // Impossible Difficulty: 45 degree rotation, watercolor filer and alpha mask 3
            _impossibleFilterGroup = new FilterGroup(new IFilter[]
            {
                FilterFactory.CreateFreeRotationFilter(45f, RotationResizeMode.FitInside),
                FilterFactory.CreateWatercolorFilter(0.8f, 1),
                FilterFactory.CreateImageFusionFilter(_maskBackground, _mask3, false)
            });

        }

        private static Bitmap LoadBitmap(string fileName)
        {
            Stream maskStream = Application.GetResourceStream(new Uri("Assets/Masks/" + fileName, UriKind.Relative)).Stream;
            BitmapImage maskImage = new BitmapImage();
            maskImage.SetSource(maskStream);
            return new WriteableBitmap(maskImage).AsBitmap();
        }

        public async Task ApplyDifficultyToImage(Stream image, Difficulty level, Image resultImage)
        {
            try
            {
                _session = await EditingSessionFactory.CreateEditingSessionAsync(image);

                switch (level)
                {
                    case Difficulty.Easy:
                        _session.AddFilter(_easyFilterGroup);
                        break;
                    case Difficulty.Moderate:
                        _session.AddFilter(_moderateFilterGroup);
                        break;
                    case Difficulty.Hard:
                        _session.AddFilter(_hardFilterGroup);
                        break;
                    case Difficulty.VeryHard:
                        _session.AddFilter(_veryHardFilterGroup);
                        break;
                    case Difficulty.ReallyHard:
                        _session.AddFilter(_impossibleFilterGroup);
                        break;
                    default:
                        break;
                }


                //Render the image to the Image control in the Quiz page
                await _session.RenderToImageAsync(resultImage);

                _session.Dispose();
                _session = null;
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while processing the image: " + e.Message);
            }

        }
    }
}
