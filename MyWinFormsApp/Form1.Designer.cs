using System.Windows.Forms.VisualStyles;

namespace MyWinFormsApp;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(400, 600);
        this.Text = "Pdf Uploader";

        Button UploadButton = new Button()
        {
            Height = 100,
            Width = 200,
            Text = "Upload",
            Location = new Point(100, 450),
            Font = new Font("Arial", 19, FontStyle.Regular)
        };

        Button BrowseFiles = new Button()
        {
            Height = 100,
            Width = 200,
            Text = "No File Selected, Browse",
            Location = new Point(100, 200),
            Font = new Font("Arial", 15, FontStyle.Regular)
        };
        BrowseFiles.Click += (sender, e) =>
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                BrowseFiles.Text = openFileDialog.SafeFileName;
                BrowseFiles.Name = openFileDialog.FileName;
            }
        };

        ComboBox Select_Cat = new ComboBox()
        {
            Height = 30,
            Width = 200,
            Font = new Font("Arial", 15, FontStyle.Regular),
            Location = new Point(100, 300),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        Select_Cat.Items.AddRange(new object[] { "Select Category", "Elektronik", "Hogtalare", "Vinyl", "Kablar" });
        Select_Cat.SelectedIndex = 0;
        
        
        ComboBox Select_SubCat = new ComboBox()
        {
            Height = 30,
            Width = 200,
            Font = new Font("Arial", 15, FontStyle.Regular),
            Location = new Point(100, 330),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Visible = false
        };
        Select_SubCat.Items.AddRange(new object[] { "Select Sub Category", "Streamer", "Dac", "Forsteg", "CD Spelare" });
        Select_SubCat.SelectedIndex = 0;
        
        ComboBox Select_Brand = new ComboBox()
        {
            Height = 30,
            Width = 200,
            Font = new Font("Arial", 15, FontStyle.Regular),
            Location = new Point(100, 360),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Visible = false
        };
        Select_Brand.Items.AddRange(new object[] { "Select Brand", "Lindemann", "Sotm", "Cambridge", "Advance" });
        Select_Brand.SelectedIndex = 0;
        
        
        Select_Cat.SelectedIndexChanged += (sender, e) =>
        {
            Select_SubCat.Visible = true;
        };
        
        Select_SubCat.SelectedIndexChanged += (sender, e) =>
        {
            Select_Brand.Visible = true;
        };
        
        //Upload File
        UploadButton.Click += async (sender, e) =>
        {
            if (Select_Cat.SelectedIndex == 0 || Select_SubCat.SelectedIndex == 0 || Select_Brand.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select All Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string url = $"https://hificonsult.lunalu.org/upload/pricelist/{Select_Cat.SelectedItem}/{Select_SubCat.SelectedItem}/{Select_Brand.SelectedItem}";
            

            try
            {
                if (BrowseFiles.Name == "")
                {
                    MessageBox.Show("Please Select a File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //await UploadFileAsync(BrowseFiles.Name, url);
                MessageBox.Show("File Uploaded Successfully", "Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error Occured please try again later. Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            };
            
        };
        
        this.Controls.Add(UploadButton);
        this.Controls.Add(BrowseFiles);
        this.Controls.Add(Select_Cat);
        this.Controls.Add(Select_SubCat);
        this.Controls.Add(Select_Brand);
    }

    private async Task UploadFileAsync(string filePath, string url)
    {
        using (HttpClient client = new HttpClient())
        using (MultipartFormDataContent content = new MultipartFormDataContent())
        {
            //Read File
            byte[] fileContent = File.ReadAllBytes(filePath);
            ByteArrayContent fileContentData = new ByteArrayContent(fileContent);
            
            // Configure Headers
            fileContentData.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            
            // Add File to form data
            content.Add(fileContentData, "file", Path.GetFileName(filePath));
            
            //Send POST
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                
                //Handle Response
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
            }
        }
    }
    #endregion
}
