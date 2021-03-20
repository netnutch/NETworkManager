﻿using MahApps.Metro.Controls.Dialogs;
using NETworkManager.Profiles;
using NETworkManager.ViewModels;
using NETworkManager.Views;
using System.Threading.Tasks;
using System.Windows;

namespace NETworkManager
{
    public static class ProfileDialogManager
    {

        #region Add profile, Edit profile, CopyAs profile, Delete profile, Edit group
        public static async Task ShowAddProfileDialog(IProfileManager viewModel, IDialogCoordinator dialogCoordinator)
        {
            var customDialog = new CustomDialog
            {
                Title = Localization.Resources.Strings.AddProfile,
                Style = (Style)Application.Current.FindResource("ProfileMetroDialog")
            };

            var profileViewModel = new ProfileViewModel(async instance =>
            {
                await dialogCoordinator.HideMetroDialogAsync(viewModel, customDialog);
                viewModel.OnProfileDialogClose();

                AddProfile(instance);
            }, async instance =>
            {
                await dialogCoordinator.HideMetroDialogAsync(viewModel, customDialog);
                viewModel.OnProfileDialogClose();
            }, ProfileManager.GetGroups());

            customDialog.Content = new ProfileDialog
            {
                DataContext = profileViewModel
            };

            viewModel.OnProfileDialogOpen();

            await dialogCoordinator.ShowMetroDialogAsync(viewModel, customDialog);
        }

        public static async Task ShowEditProfileDialog(IProfileManager viewModel, IDialogCoordinator dialogCoordinator, ProfileInfo selectedProfile)
        {
            var customDialog = new CustomDialog
            {
                Title = Localization.Resources.Strings.EditProfile,
                Style = (Style)Application.Current.FindResource("ProfileMetroDialog")
            };

            var profileViewModel = new ProfileViewModel(async instance =>
            {
                await dialogCoordinator.HideMetroDialogAsync(viewModel, customDialog);
                viewModel.OnProfileDialogClose();

                ProfileManager.RemoveProfile(selectedProfile);

                AddProfile(instance);
            }, async instance =>
            {
                await dialogCoordinator.HideMetroDialogAsync(viewModel, customDialog);
                viewModel.OnProfileDialogClose();
            }, ProfileManager.GetGroups(), ProfileEditMode.Edit, selectedProfile);

            customDialog.Content = new ProfileDialog
            {
                DataContext = profileViewModel
            };

            viewModel.OnProfileDialogOpen();
            await dialogCoordinator.ShowMetroDialogAsync(viewModel, customDialog);
        }

        public static async Task ShowCopyAsProfileDialog(IProfileManager viewModel, IDialogCoordinator dialogCoordinator, ProfileInfo selectedProfile)
        {
            var customDialog = new CustomDialog
            {
                Title = Localization.Resources.Strings.CopyProfile,
                Style = (Style)Application.Current.FindResource("ProfileMetroDialog")
            };

            var profileViewModel = new ProfileViewModel(async instance =>
            {
                await dialogCoordinator.HideMetroDialogAsync(viewModel, customDialog);
                viewModel.OnProfileDialogClose();

                AddProfile(instance);
            }, async instance =>
            {
                await dialogCoordinator.HideMetroDialogAsync(viewModel, customDialog);
                viewModel.OnProfileDialogClose();
            }, ProfileManager.GetGroups(), ProfileEditMode.Copy, selectedProfile);

            customDialog.Content = new ProfileDialog
            {
                DataContext = profileViewModel
            };

            viewModel.OnProfileDialogOpen();
            await dialogCoordinator.ShowMetroDialogAsync(viewModel, customDialog);
        }

        public static async Task ShowDeleteProfileDialog(IProfileManager viewModel, IDialogCoordinator dialogCoordinator, ProfileInfo selectedProfile)
        {
            var customDialog = new CustomDialog
            {
                Title = Localization.Resources.Strings.DeleteProfile
            };

            var confirmDeleteViewModel = new ConfirmDeleteViewModel(async instance =>
            {
                await dialogCoordinator.HideMetroDialogAsync(viewModel, customDialog);
                viewModel.OnProfileDialogClose();

                ProfileManager.RemoveProfile(selectedProfile);
            }, async instance =>
            {
                await dialogCoordinator.HideMetroDialogAsync(viewModel, customDialog);
                viewModel.OnProfileDialogClose();
            }, Localization.Resources.Strings.DeleteProfileMessage);

            customDialog.Content = new ConfirmDeleteDialog
            {
                DataContext = confirmDeleteViewModel
            };

            viewModel.OnProfileDialogOpen();
            await dialogCoordinator.ShowMetroDialogAsync(viewModel, customDialog);
        }

        public static async Task ShowEditGroupDialog(IProfileManager viewModel, IDialogCoordinator dialogCoordinator, string group)
        {
            var customDialog = new CustomDialog
            {
                Title = Localization.Resources.Strings.EditGroup
            };

            var editGroupViewModel = new GroupViewModel(async instance =>
            {
                await dialogCoordinator.HideMetroDialogAsync(viewModel, customDialog);
                viewModel.OnProfileDialogClose();

                ProfileManager.RenameGroup(instance.OldGroup, instance.Group);

                viewModel.RefreshProfiles();
            }, async instance =>
            {
                await dialogCoordinator.HideMetroDialogAsync(viewModel, customDialog);
                viewModel.OnProfileDialogClose();
            }, group, ProfileManager.GetGroups());

            customDialog.Content = new GroupDialog
            {
                DataContext = editGroupViewModel
            };

            viewModel.OnProfileDialogOpen();
            await dialogCoordinator.ShowMetroDialogAsync(viewModel, customDialog);
        }
        #endregion

        public static void AddProfile(ProfileViewModel instance)
        {
            ProfileManager.AddProfile(new ProfileInfo
            {
                Name = instance.Name?.Trim(),
                Host = instance.Host?.Trim(),
                Group = instance.Group?.Trim(),
                Tags = instance.Tags?.Trim(),

                NetworkInterface_Enabled = instance.NetworkInterface_Enabled,
                NetworkInterface_EnableStaticIPAddress = instance.NetworkInterface_EnableStaticIPAddress,
                NetworkInterface_IPAddress = instance.NetworkInterface_IPAddress?.Trim(),
                NetworkInterface_Gateway = instance.NetworkInterface_Gateway?.Trim(),
                NetworkInterface_SubnetmaskOrCidr = instance.NetworkInterface_SubnetmaskOrCidr?.Trim(),
                NetworkInterface_EnableStaticDNS = instance.NetworkInterface_EnableStaticDNS,
                NetworkInterface_PrimaryDNSServer = instance.NetworkInterface_PrimaryDNSServer?.Trim(),
                NetworkInterface_SecondaryDNSServer = instance.NetworkInterface_SecondaryDNSServer?.Trim(),

                IPScanner_Enabled = instance.IPScanner_Enabled,
                IPScanner_InheritHost = instance.IPScanner_InheritHost,
                IPScanner_HostOrIPRange = instance.IPScanner_InheritHost ? instance.Host?.Trim() : instance.IPScanner_HostOrIPRange?.Trim(),

                PortScanner_Enabled = instance.PortScanner_Enabled,
                PortScanner_InheritHost = instance.PortScanner_InheritHost,
                PortScanner_Host = instance.PortScanner_InheritHost ? instance.Host?.Trim() : instance.PortScanner_Host?.Trim(),
                PortScanner_Ports = instance.PortScanner_Ports?.Trim(),

                PingMonitor_Enabled = instance.PingMonitor_Enabled,
                PingMonitor_InheritHost = instance.PingMonitor_InheritHost,
                PingMonitor_Host = instance.PingMonitor_InheritHost ? instance.Host?.Trim() : instance.PingMonitor_Host?.Trim(),

                Traceroute_Enabled = instance.Traceroute_Enabled,
                Traceroute_InheritHost = instance.Traceroute_InheritHost,
                Traceroute_Host = instance.Traceroute_InheritHost ? instance.Host?.Trim() : instance.Traceroute_Host?.Trim(),

                DNSLookup_Enabled = instance.DNSLookup_Enabled,
                DNSLookup_InheritHost = instance.Traceroute_InheritHost,
                DNSLookup_Host = instance.DNSLookup_InheritHost ? instance.Host?.Trim() : instance.DNSLookup_Host?.Trim(),

                RemoteDesktop_Enabled = instance.RemoteDesktop_Enabled,
                RemoteDesktop_InheritHost = instance.RemoteDesktop_InheritHost,
                RemoteDesktop_Host = instance.RemoteDesktop_InheritHost ? instance.Host?.Trim() : instance.RemoteDesktop_Host?.Trim(),
                RemoteDesktop_Username = instance.RemoteDesktop_Username,
                RemoteDesktop_Password = instance.RemoteDesktop_Password,
                RemoteDesktop_OverrideDisplay = instance.RemoteDesktop_OverrideDisplay,
                RemoteDesktop_AdjustScreenAutomatically = instance.RemoteDesktop_AdjustScreenAutomatically,
                RemoteDesktop_UseCurrentViewSize = instance.RemoteDesktop_UseCurrentViewSize,
                RemoteDesktop_UseFixedScreenSize = instance.RemoteDesktop_UseFixedScreenSize,
                RemoteDesktop_ScreenWidth = instance.RemoteDesktop_ScreenWidth,
                RemoteDesktop_ScreenHeight = instance.RemoteDesktop_ScreenHeight,
                RemoteDesktop_UseCustomScreenSize = instance.RemoteDesktop_UseCustomScreenSize,
                RemoteDesktop_CustomScreenWidth = int.Parse(instance.RemoteDesktop_CustomScreenWidth),
                RemoteDesktop_CustomScreenHeight = int.Parse(instance.RemoteDesktop_CustomScreenHeight),
                RemoteDesktop_OverrideColorDepth = instance.RemoteDesktop_OverrideColorDepth,
                RemoteDesktop_ColorDepth = instance.RemoteDesktop_SelectedColorDepth,
                RemoteDesktop_OverridePort = instance.RemoteDesktop_OverridePort,
                RemoteDesktop_Port = instance.RemoteDesktop_Port,
                RemoteDesktop_OverrideCredSspSupport = instance.RemoteDesktop_OverrideCredSspSupport,
                RemoteDesktop_EnableCredSspSupport = instance.RemoteDesktop_EnableCredSspSupport,
                RemoteDesktop_OverrideAuthenticationLevel = instance.RemoteDesktop_OverrideAuthenticationLevel,
                RemoteDesktop_AuthenticationLevel = instance.RemoteDesktop_AuthenticationLevel,
                RemoteDesktop_OverrideAudioRedirectionMode = instance.RemoteDesktop_OverrideAudioRedirectionMode,
                RemoteDesktop_AudioRedirectionMode = instance.RemoteDesktop_AudioRedirectionMode,
                RemoteDesktop_OverrideAudioCaptureRedirectionMode = instance.RemoteDesktop_OverrideAudioCaptureRedirectionMode,
                RemoteDesktop_AudioCaptureRedirectionMode = instance.RemoteDesktop_AudioCaptureRedirectionMode,
                RemoteDesktop_OverrideApplyWindowsKeyCombinations = instance.RemoteDesktop_OverrideApplyWindowsKeyCombinations,
                RemoteDesktop_KeyboardHookMode = instance.RemoteDesktop_KeyboardHookMode,
                RemoteDesktop_OverrideRedirectClipboard = instance.RemoteDesktop_OverrideRedirectClipboard,
                RemoteDesktop_RedirectClipboard = instance.RemoteDesktop_RedirectClipboard,
                RemoteDesktop_OverrideRedirectDevices = instance.RemoteDesktop_OverrideRedirectDevices,
                RemoteDesktop_RedirectDevices = instance.RemoteDesktop_RedirectDevices,
                RemoteDesktop_OverrideRedirectDrives = instance.RemoteDesktop_OverrideRedirectDrives,
                RemoteDesktop_RedirectDrives = instance.RemoteDesktop_RedirectDrives,
                RemoteDesktop_OverrideRedirectPorts = instance.RemoteDesktop_OverrideRedirectPorts,
                RemoteDesktop_RedirectPorts = instance.RemoteDesktop_RedirectPorts,
                RemoteDesktop_OverrideRedirectSmartcards = instance.RemoteDesktop_OverrideRedirectSmartcards,
                RemoteDesktop_RedirectSmartCards = instance.RemoteDesktop_RedirectSmartCards,
                RemoteDesktop_OverrideRedirectPrinters = instance.RemoteDesktop_OverrideRedirectPrinters,
                RemoteDesktop_RedirectPrinters = instance.RemoteDesktop_RedirectPrinters,
                RemoteDesktop_OverridePersistentBitmapCaching = instance.RemoteDesktop_OverridePersistentBitmapCaching,
                RemoteDesktop_PersistentBitmapCaching = instance.RemoteDesktop_PersistentBitmapCaching,
                RemoteDesktop_OverrideReconnectIfTheConnectionIsDropped = instance.RemoteDesktop_OverrideReconnectIfTheConnectionIsDropped,
                RemoteDesktop_ReconnectIfTheConnectionIsDropped = instance.RemoteDesktop_ReconnectIfTheConnectionIsDropped,
                RemoteDesktop_OverrideNetworkConnectionType = instance.RemoteDesktop_OverrideNetworkConnectionType,
                RemoteDesktop_NetworkConnectionType = instance.RemoteDesktop_NetworkConnectionType,
                RemoteDesktop_DesktopBackground = instance.RemoteDesktop_DesktopBackground,
                RemoteDesktop_FontSmoothing = instance.RemoteDesktop_FontSmoothing,
                RemoteDesktop_DesktopComposition = instance.RemoteDesktop_DesktopComposition,
                RemoteDesktop_ShowWindowContentsWhileDragging = instance.RemoteDesktop_ShowWindowContentsWhileDragging,
                RemoteDesktop_MenuAndWindowAnimation = instance.RemoteDesktop_MenuAndWindowAnimation,
                RemoteDesktop_VisualStyles = instance.RemoteDesktop_VisualStyles,

                PowerShell_Enabled = instance.PowerShell_Enabled,
                PowerShell_EnableRemoteConsole = instance.PowerShell_EnableRemoteConsole,
                PowerShell_InheritHost = instance.PowerShell_InheritHost,
                PowerShell_Host = instance.PowerShell_InheritHost ? instance.Host?.Trim() : instance.PowerShell_Host?.Trim(),
                PowerShell_OverrideAdditionalCommandLine = instance.PowerShell_OverrideAdditionalCommandLine,
                PowerShell_AdditionalCommandLine = instance.PowerShell_AdditionalCommandLine,
                PowerShell_OverrideExecutionPolicy = instance.PowerShell_OverrideExecutionPolicy,
                PowerShell_ExecutionPolicy = instance.PowerShell_ExecutionPolicy,

                PuTTY_Enabled = instance.PuTTY_Enabled,
                PuTTY_ConnectionMode = instance.PuTTY_ConnectionMode,
                PuTTY_InheritHost = instance.PuTTY_InheritHost,
                PuTTY_HostOrSerialLine = instance.PuTTY_ConnectionMode == Models.PuTTY.ConnectionMode.Serial ? instance.PuTTY_HostOrSerialLine?.Trim() : (instance.PuTTY_InheritHost ? instance.Host?.Trim() : instance.PuTTY_HostOrSerialLine?.Trim()),
                PuTTY_OverridePortOrBaud = instance.PuTTY_OverridePortOrBaud,
                PuTTY_PortOrBaud = instance.PuTTY_PortOrBaud,
                PuTTY_OverrideUsername = instance.PuTTY_OverrideUsername,
                PuTTY_Username = instance.PuTTY_Username?.Trim(),
                PuTTY_OverridePrivateKeyFile = instance.PuTTY_OverridePrivateKeyFile,
                PuTTY_PrivateKeyFile = instance.PuTTY_PrivateKeyFile,
                PuTTY_OverrideProfile = instance.PuTTY_OverrideProfile,
                PuTTY_Profile = instance.PuTTY_Profile?.Trim(),
                PuTTY_OverrideEnableLog = instance.PuTTY_OverrideEnableLog,
                PuTTY_EnableLog = instance.PuTTY_EnableLog,
                PuTTY_OverrideLogMode = instance.PuTTY_OverrideLogMode,
                PuTTY_LogMode = instance.PuTTY_LogMode,
                PuTTY_OverrideLogPath = instance.PuTTY_OverrideLogPath,
                PuTTY_LogPath = instance.PuTTY_LogPath,
                PuTTY_OverrideLogFileName = instance.PuTTY_OverrideLogFileName,
                PuTTY_LogFileName = instance.PuTTY_LogFileName,
                PuTTY_OverrideAdditionalCommandLine = instance.PuTTY_OverrideAdditionalCommandLine,
                PuTTY_AdditionalCommandLine = instance.PuTTY_AdditionalCommandLine?.Trim(),

                TigerVNC_Enabled = instance.TigerVNC_Enabled,
                TigerVNC_InheritHost = instance.TigerVNC_InheritHost,
                TigerVNC_Host = instance.TigerVNC_InheritHost ? instance.Host?.Trim() : instance.TigerVNC_Host?.Trim(),
                TigerVNC_OverridePort = instance.TigerVNC_OverridePort,
                TigerVNC_Port = instance.TigerVNC_Port,

                WebConsole_Enabled = instance.WebConsole_Enabled,
                WebConsole_Url = instance.WebConsole_Url,

                WakeOnLAN_Enabled = instance.WakeOnLAN_Enabled,
                WakeOnLAN_MACAddress = instance.WakeOnLAN_MACAddress?.Trim(),
                WakeOnLAN_Broadcast = instance.WakeOnLAN_Broadcast?.Trim(),
                WakeOnLAN_OverridePort = instance.WakeOnLAN_OverridePort,
                WakeOnLAN_Port = instance.WakeOnLAN_Port,

                Whois_Enabled = instance.Whois_Enabled,
                Whois_InheritHost = instance.Whois_InheritHost,
                Whois_Domain = instance.Whois_InheritHost ? instance.Host?.Trim() : instance.Whois_Domain?.Trim()
            });
        }
    }
}
