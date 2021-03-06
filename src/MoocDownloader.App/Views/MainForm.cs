﻿using MoocDownloader.App.Models;
using MoocDownloader.App.Utilities;
using MoocDownloader.App.ViewModels;
using System;
using System.IO;
using System.Windows.Forms;

namespace MoocDownloader.App.Views
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// View model.
        /// </summary>
        private readonly MainViewModel _viewModel;

        public MainForm()
        {
            InitializeComponent();

            _viewModel = new MainViewModel
            {
                Log              = Log,
                SetStatus        = SetStatusText,
                SetUIStatus      = SetUIStatus,
                UpdateCurrentBar = UpdateCurrentProgressBar,
                UpdateTotalBar   = UpdateTotalProgressBar,
                ResetCurrentBar  = ResetCurrentProgressBar,
                ResetTotalBar    = ResetTotalProgressBar
            };
        }

        /// <summary>
        /// Login icourse163.org.
        /// </summary>
        private void LoginMoocButton_Click(object sender, EventArgs e)
        {
            _viewModel.LoginMooc();
        }

        /// <summary>
        /// clear course url text.
        /// </summary>
        private void ClearCourseUrlButton_Click(object sender, EventArgs e)
        {
            CourseUrlTextBox.Text = string.Empty;
        }

        /// <summary>
        /// Find save file path.
        /// </summary>
        private void FindPathButton_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                SavePathTextBox.Text = dialog.SelectedPath;
                Log($@"已设置保存路径为 {SavePathTextBox.Text}.");
            }
        }

        /// <summary>
        /// window closing.
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(@"确认关闭程序.", @"提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
             != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Write log.
        /// </summary>
        /// <param name="message">log message.</param>
        private void Log(string message)
        {
            RunningLogListBox.Items.Add($"{DateTime.Now:hh:mm:ss} {message}");
        }

        /// <summary>
        /// Start download.
        /// </summary>
        private void StartDownloadButton_Click(object sender, EventArgs e)
        {
            _viewModel.StartDownload();
        }

        /// <summary>
        /// Cancel download.
        /// </summary>
        private void CancelDownloadButton_Click(object sender, EventArgs e)
        {
            _viewModel.CancelDownload();
        }

        private void SetStatusText(string text)
        {
            StatusToolStripStatusLabel.Text = text;
        }

        private void UpdateCurrentProgressBar(int value)
        {
            CurrentToolStripProgressBar.Value = value;
        }

        private void ResetCurrentProgressBar()
        {
            CurrentToolStripProgressBar.Value = 0;
        }

        private void UpdateTotalProgressBar(int value)
        {
            TotalStripProgressBar.Value = value;
        }

        private void ResetTotalProgressBar()
        {
            TotalStripProgressBar.Value = 0;
        }

        /// <summary>
        /// Set whether interface controls are disabled.
        /// </summary>
        /// <param name="isDisable">Is disabled.</param>
        private void SetUIStatus(bool isDisable)
        {
            SettingToolStripMenuItem.Enabled = isDisable;

            StartDownloadToolStripMenuItem.Enabled  = isDisable;
            CancelDownloadToolStripMenuItem.Enabled = !isDisable;

            LoginMoocButton.Enabled      = isDisable;
            ClearCourseUrlButton.Enabled = isDisable;
            CourseUrlTextBox.Enabled     = isDisable;
            SavePathTextBox.Enabled      = isDisable;
            FindPathButton.Enabled       = isDisable;

            DownloadVideoCheckBox.Enabled      = isDisable;
            DownloadAttachmentCheckBox.Enabled = isDisable;
            DownloadDocumentCheckBox.Enabled   = isDisable;
            DownloadSubtitleCheckBox.Enabled   = isDisable;

            SDRadioButton.Enabled  = isDisable;
            HDRadioButton.Enabled  = isDisable;
            UHDRadioButton.Enabled = isDisable;

            StartDownloadButton.Enabled  = isDisable;
            CancelDownloadButton.Enabled = !isDisable;
        }

        #region UI controls properties binding.

        private void CourseUrlTextBox_TextChanged(object sender, EventArgs e)
        {
            _viewModel.SetCourseUrl(CourseUrlTextBox.Text);
        }

        private void SavePathTextBox_TextChanged(object sender, EventArgs e)
        {
            _viewModel.SetSavePath(SavePathTextBox.Text);
        }

        private void DownloadVideoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            DownloadVideoToolStripMenuItem.CheckState = DownloadVideoCheckBox.CheckState;
            _viewModel.SetDownloadVideo(DownloadVideoCheckBox.Checked);
        }

        private void DownloadDocumentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            DownloadDocumentToolStripMenuItem.CheckState = DownloadDocumentCheckBox.CheckState;
            _viewModel.SetDownloadDocument(DownloadDocumentCheckBox.Checked);
        }

        private void DownloadSubtitleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            DownloadSubtitlesToolStripMenuItem.CheckState = DownloadSubtitleCheckBox.CheckState;
            _viewModel.SetDownloadSubtitle(DownloadSubtitleCheckBox.Checked);
        }

        private void DownloadAttachmentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            DownloadAttachmentToolStripMenuItem.CheckState = DownloadAttachmentCheckBox.CheckState;
            _viewModel.SetDownloadAttachment(DownloadAttachmentCheckBox.Checked);
        }

        private void SDRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (SDRadioButton.Checked)
            {
                _viewModel.SetVideoQuality(VideoQuality.SD);
                HDToolStripMenuItem.CheckState  = CheckState.Unchecked;
                SDToolStripMenuItem.CheckState  = CheckState.Checked;
                UHDToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void HDRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (HDRadioButton.Checked)
            {
                _viewModel.SetVideoQuality(VideoQuality.HD);
                HDToolStripMenuItem.CheckState  = CheckState.Checked;
                SDToolStripMenuItem.CheckState  = CheckState.Unchecked;
                UHDToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void UHDRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (UHDRadioButton.Checked)
            {
                _viewModel.SetVideoQuality(VideoQuality.UHD);
                HDToolStripMenuItem.CheckState  = CheckState.Unchecked;
                SDToolStripMenuItem.CheckState  = CheckState.Unchecked;
                UHDToolStripMenuItem.CheckState = CheckState.Checked;
            }
        }

        #endregion

        #region HELP MENU

        /// <summary>
        /// When the program is started
        /// </summary>
        private async void MainForm_Load(object sender, EventArgs e)
        {
            // set the course save path.
            SavePathTextBox.Text = Path.Combine(Application.StartupPath, "课程下载");

            // Check if there is a new version.
            var currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            var update         = new VersionUpdate();

            var newVersionInfo = await update.GetNewVersionAsync();
            var version        = Version.Parse(newVersionInfo.Version);

            if (version > currentVersion)
            {
                /**
                 * exist new version.
                 *
                 * https://docs.microsoft.com/en-us/dotnet/standard/assembly/versioning
                 * <主版本>.<次版本>.<生成号>.<修订版本>
                 * <major version>.<minor version>.<build number>.<revision>
                 */
                var form = new VersionForm(newVersionInfo);

                form.ShowDialog();
            }
        }

        /// <summary>
        /// About
        /// </summary>
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutWindow = new AboutForm();

            aboutWindow.ShowDialog();
        }

        /// <summary>
        /// Feedback.
        /// </summary>
        private void FeedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string ISSUES_URL = "https://github.com/xixixixixiao/mooc-downloader/issues";

            System.Diagnostics.Process.Start(ISSUES_URL);
        }

        /// <summary>
        /// Update.
        /// </summary>
        private void UpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string RELEASES_URL = "https://github.com/xixixixixiao/mooc-downloader/releases";

            System.Diagnostics.Process.Start(RELEASES_URL);
        }

        /// <summary>
        /// Help.
        /// </summary>
        private void ViewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string INDEX_URL = "https://github.com/xixixixixiao/mooc-downloader";

            System.Diagnostics.Process.Start(INDEX_URL);
        }

        #endregion

        #region START MENU

        /// <summary>
        /// Start.
        /// </summary>
        private void StartDownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _viewModel.StartDownload();
        }

        /// <summary>
        /// Cancel.
        /// </summary>
        private void CancelDownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _viewModel.CancelDownload();
        }

        /// <summary>
        /// Exit app.
        /// </summary>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StartDownloadButton.Enabled)
            {
                Environment.Exit(0);
            }

            if (MessageBox.Show($@"正在下载, 是否退出?", @"提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
             == DialogResult.OK)
            {
                Environment.Exit(0);
            }
        }

        #endregion

        #region SETTING MENU

        /// <summary>
        /// Login.
        /// </summary>
        private void LoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _viewModel.LoginMooc();
        }

        /// <summary>
        /// Paste course link.
        /// </summary>
        private void PasteCourseLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var data = Clipboard.GetDataObject();
            if (data?.GetDataPresent(DataFormats.Text) ?? false)
            {
                var url = (string) data.GetData(DataFormats.Text);
                if (Uri.TryCreate(url, UriKind.Absolute, out _))
                {
                    CourseUrlTextBox.Text = url;
                }
                else
                {
                    MessageBox.Show(@"复制的链接有误.", @"提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// Set course saved path.
        /// </summary>
        private void SetCourseSavePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindPathButton_Click(sender, e);
        }

        /// <summary>
        /// Whether to download videos.
        /// </summary>
        private void DownloadVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DownloadVideoCheckBox.CheckState = DownloadVideoToolStripMenuItem.CheckState;
        }

        /// <summary>
        /// Whether to download attachments.
        /// </summary>
        private void DownloadAttachmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DownloadAttachmentCheckBox.CheckState = DownloadAttachmentToolStripMenuItem.CheckState;
        }

        /// <summary>
        /// Whether to download documents.
        /// </summary>
        private void DownloadDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DownloadDocumentCheckBox.CheckState = DownloadDocumentToolStripMenuItem.CheckState;
        }

        /// <summary>
        /// Whether to download subtitles.
        /// </summary>
        private void DownloadSubtitlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DownloadSubtitleCheckBox.CheckState = DownloadSubtitlesToolStripMenuItem.CheckState;
        }

        /// <summary>
        /// Set video quality to UHD.
        /// </summary>
        private void UHDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UHDToolStripMenuItem.CheckState = CheckState.Checked;
            HDToolStripMenuItem.CheckState  = CheckState.Unchecked;
            SDToolStripMenuItem.CheckState  = CheckState.Unchecked;

            UHDRadioButton.Checked = true;
            HDRadioButton.Checked  = false;
            SDRadioButton.Checked  = false;

            _viewModel.SetVideoQuality(VideoQuality.UHD);
        }

        /// <summary>
        /// Set video quality to HD.
        /// </summary>
        private void HDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UHDToolStripMenuItem.CheckState = CheckState.Unchecked;
            HDToolStripMenuItem.CheckState  = CheckState.Checked;
            SDToolStripMenuItem.CheckState  = CheckState.Unchecked;

            UHDRadioButton.Checked = false;
            HDRadioButton.Checked  = true;
            SDRadioButton.Checked  = false;

            _viewModel.SetVideoQuality(VideoQuality.UHD);
        }

        /// <summary>
        /// Set video quality to SD.
        /// </summary>
        private void SDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UHDToolStripMenuItem.CheckState = CheckState.Unchecked;
            HDToolStripMenuItem.CheckState  = CheckState.Unchecked;
            SDToolStripMenuItem.CheckState  = CheckState.Checked;

            UHDRadioButton.Checked = false;
            HDRadioButton.Checked  = false;
            SDRadioButton.Checked  = true;

            _viewModel.SetVideoQuality(VideoQuality.UHD);
        }

        #endregion
    }
}