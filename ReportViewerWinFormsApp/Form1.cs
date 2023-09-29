using DAL.Model;
using DAL.Services;

namespace ReportViewerWinFormsApp
{
    public partial class Form1 : Form
    {
        private readonly ConferenceService conferenceService = new();
        private readonly UserService userService = new();
        private const string showSchedule = "Show conference schedule";
        private const string showParticipants = "Show conference participants";
        private const string showEquipment = "Show conference equipment";
        private Task savePhotoTask = null;
        private string newPathToPhoto = "";

        public Form1()
        {
            InitializeComponent();
            InitUi();
        }

        private async Task InitUi()
        {
            reportsComboBox.Items.Insert(0, showSchedule);
            reportsComboBox.Items.Insert(1, showParticipants);
            reportsComboBox.Items.Insert(2, showEquipment);
            reportsComboBox.SelectedIndex = 0;
            List<string> conferencesNames = await conferenceService.GetConferencesNames();
            for (int i = 0; i < conferencesNames.Count; i++)
            {
                conferenceComboBox.Items.Insert(i, conferencesNames[i]);
            }
            conferenceComboBox.SelectedIndex = 0;
            for (int i = 0; i < 4; i++)
            {
                reportDataGridView.Columns.Add("", "");
            }
            saveUserProgressBar.Maximum = 20;
        }

        private async void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                reportDataGridView.Rows.Clear();
                switch (reportsComboBox.SelectedItem)
                {
                    case showSchedule: await ShowSchedule(); break;
                    case showParticipants: await ShowParticipants(); break;
                    case showEquipment: await ShowEquipment(); break;
                    default:
                        break;
                }
            }
            catch (InvalidOperationException)
            {
                ShowError("Conference not found");
            }
            catch (FileNotFoundException)
            {
                ShowError("Photo not found");
            }
            catch (Exception)
            {
                ShowError("Something went wrong");
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK);
        }

        private async Task<Conference> GetConference()
        {
            Conference? conference = await conferenceService.GetConferenceByName(
                conferenceComboBox.SelectedItem.ToString() ?? ""
            );
            return conference ?? throw new InvalidOperationException();
        }

        private async Task ShowSchedule()
        {
            foreach (Section section in (await GetConference()).Sections)
            {
                reportDataGridView.Rows.Add(
                    $"Section: {section.Name}",
                    $"Chairperson: {section.Chairperson.FullName}",
                    $"Room: {section.Room}"
                );
                foreach (Talk talk in section.Talks)
                {
                    reportDataGridView.Rows.Add(
                        "",
                        $"Talk: {talk.Theme}",
                        talk.Speaker.FullName,
                        $"{talk.DateStart.ToShortTimeString()} - {talk.DateEnd.ToShortTimeString()}"
                    );
                }
            }
        }

        private async Task ShowParticipants()
        {
            Conference conf = await GetConference();
            reportDataGridView.Rows.Add("Chairpersons:");
            foreach (Section section in conf.Sections)
            {
                reportDataGridView.Rows.Add(
                    "",
                    $"{section.Chairperson.FullName}",
                    $"{section.Chairperson.Work}",
                    $"{section.Chairperson.Position}"
                );
            }
            reportDataGridView.Rows.Add("Speakers:");
            foreach (Section section in conf.Sections)
            {
                foreach (Talk talk in section.Talks)
                {
                    reportDataGridView.Rows.Add(
                        "",
                        $"{talk.Speaker.FullName}",
                        $"{talk.Speaker.Work}",
                        $"{talk.Speaker.Position}");
                }
            }
        }

        private async Task ShowEquipment()
        {
            HashSet<Equipment> equipment = new();
            foreach (Section section in (await GetConference()).Sections)
            {
                foreach (Talk talk in section.Talks)
                {
                    foreach (Equipment equip in talk.Equipment)
                    {
                        equipment.Add(equip);
                    }
                }
            }
            reportDataGridView.Rows.Add("Needed equipment for the conference:");
            foreach (Equipment equip in equipment)
            {
                reportDataGridView.Rows.Add(equip.Name);
                foreach (Section section in (await GetConference()).Sections)
                {
                    foreach (Talk talk in section.Talks)
                    {
                        if (talk.Equipment.Contains(equip))
                        {
                            reportDataGridView.Rows.Add(
                                "",
                                $"Room: {section.Room}",
                                $"Time: {talk.DateStart.ToShortTimeString()} - {talk.DateEnd.ToShortTimeString()}"
                            );
                        }
                    }
                }
            }
        }

        private void choosePhotoButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog photoPickFileDialog = new();
            photoPickFileDialog.Filter = "*.jpg|*.png";
            DialogResult result = photoPickFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                photoPathTextBox.Text = photoPickFileDialog.FileName;
            }
        }

        private async void saveUserButton_Click(object sender, EventArgs e)
        {
            Guid newUserId = Guid.NewGuid();
            statusLabel.Text = "Saving user...";
            await userService.AddUser(
                newUserId,
                fullNameTextBox.Text,
                degreeTextBox.Text,
                workTextBox.Text,
                positonTextBox.Text,
                bioRichTextBox.Text
            );
            newPathToPhoto = GetNewUserPathToPhoto(fullNameTextBox.Text, newUserId.ToString(), photoPathTextBox.Text);
            savePhotoTask = SavePhoto(photoPathTextBox.Text, newPathToPhoto);
            await savePhotoTask;
            await userService.UpdateUserPhoto(newUserId.ToString(), newPathToPhoto);
            statusLabel.Text = "User saved";
        }

        private async Task SavePhoto(string pathToPhoto, string newPathToPhoto)
        {
            if (!File.Exists(pathToPhoto))
            {
                throw new FileNotFoundException();
            }
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "photos"));
            File.Copy(pathToPhoto, newPathToPhoto);
            foreach (int i in Enumerable.Range(0, 20)) // Simulation of long running task.
            {
                saveUserProgressBar.Value = i;
                Console.WriteLine(savePhotoTask?.Status);
                await Task.Delay(500);
            }
            saveUserProgressBar.Value = 0;
        }

        private string GetNewUserPathToPhoto(string fullName, string id, string pathToPhoto)
        {
            string extension = Path.GetExtension(pathToPhoto);
            string newPhotoName = fullName.Trim().ToLower().Replace(' ', '_') + "_" + id;
            string[] paths = { AppDomain.CurrentDomain.BaseDirectory, "photos", newPhotoName };
            string newPath = Path.Combine(paths);
            newPath = Path.ChangeExtension(newPath, extension);
            return newPath;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (savePhotoTask?.Status == TaskStatus.WaitingForActivation)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "The photo is currently uploading to the server. Exit anyway?", 
                    "", 
                    MessageBoxButtons.YesNo
                );
                e.Cancel = dialogResult == DialogResult.No;
                if (dialogResult == DialogResult.Yes)
                {
                    if (File.Exists(newPathToPhoto))
                    {
                        File.Delete(newPathToPhoto);
                    }
                    Environment.Exit(0);
                }
            }
        }
    }
}
