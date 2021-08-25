using EigMedicoes.Win.Properties;
using System;
using System.Windows.Forms;

namespace EigMedicoes.Win {

    public static class ControlExtensions {

        private static PictureBox LoadingImage {
            get {
                return new PictureBox() {
                    Image = Resources.ajax_loader,
                    SizeMode = PictureBoxSizeMode.AutoSize
                };
            }
        }

        private static PictureBox ErrorImage {
            get {
                return new PictureBox() {
                    Image = Resources.error,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 16,
                    Height = 16
                };
            }
        }

        public static void EnterLoadingState(this Control control) {
            var image = LoadingImage;
            image.Name = "loadingImage_" + control.Name;
            image.Location = control.Location;
            control.Parent.Controls.Add(
                image);
            image.BringToFront();
            control.Enabled = false;
            control.Parent.Refresh();
        }

        public static void ExitLoadingState(this Control control) {
            var image = control.Parent.Controls["loadingImage_" + control.Name];

            if (image != null) {
                control.Parent.Controls.Remove(image);
                image.Dispose();
                image = null;
            }
            control.Enabled = true;
        }

        public static void SetErrorState(this Control control, Exception ex) {
            if (ex != null) {
                var image = ErrorImage;
                image.Name = "errorImage_" + control.Name;
                image.Location = control.Location;
                image.Cursor = Cursors.Help;

                image.Click += (object sender, EventArgs e) => MessageBox.Show(control.FindForm(), ex.ToString());

                control.Parent.Controls.Add(
                    image);
                image.BringToFront();
            } else {
                var image = control.Parent.Controls["errorImage_" + control.Name];

                if (image != null) {
                    control.Parent.Controls.Remove(image);
                    image.Dispose();
                    image = null;
                }
            }
            control.Enabled = false;
            control.Parent.Refresh();
        }
    }


}