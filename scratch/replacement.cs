        #region Integrations (Memory Cleaners & Winhance)

        private async Task LaunchIntegrationsIfNeeded(string logIdent)
        {
            // Windows Memory Compression
            if (App.Settings.Prop.CompressRamEnabled)
            {
                App.Logger.WriteLine(logIdent, "Applying RAM Compression...");
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = "-WindowStyle Hidden -Command "Enable-MMAgent -mc"",
                        UseShellExecute = true,
                        Verb = "runas",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    });
                }
                catch (Exception ex)
                {
                    App.Logger.WriteLine(logIdent, "Failed to apply RAM Compression: " + ex.Message);
                }
            }

            // Winhance
            if (App.Settings.Prop.WinhanceEnabled)
            {
                try
                {
                    string winhanceDir = Path.Combine(Paths.Base, "Winhance");
                    string winhanceExe = Path.Combine(winhanceDir, "Winhance.exe");

                    if (!File.Exists(winhanceExe))
                    {
                        App.Logger.WriteLine(logIdent, "Winhance not found, downloading...");
                        SetStatus("Downloading Winhance...");
                        Directory.CreateDirectory(winhanceDir);
                        using var http = new HttpClient();
                        http.DefaultRequestHeaders.UserAgent.ParseAdd("StarStrap/1.0");
                        var response = await http.GetAsync("https://github.com/memstechtips/Winhance/releases/latest/download/Winhance.exe");
                        response.EnsureSuccessStatusCode();
                        await using var fs = File.Create(winhanceExe);
                        await response.Content.CopyToAsync(fs);
                    }

                    if (File.Exists(winhanceExe))
                    {
                        App.Logger.WriteLine(logIdent, "Launching Winhance...");
                        Process.Start(new ProcessStartInfo { FileName = winhanceExe, WorkingDirectory = winhanceDir, UseShellExecute = true });
                    }
                }
                catch (Exception ex)
                {
                    App.Logger.WriteLine(logIdent, "Failed to launch Winhance: " + ex.Message);
                }
            }

            // Memory Cleaner
            if (App.Settings.Prop.SelectedMemoryCleaner == StarStrap.Enums.MemoryCleanerType.MemReduct)
            {
                await LaunchMemReduct(logIdent);
            }
            else if (App.Settings.Prop.SelectedMemoryCleaner == StarStrap.Enums.MemoryCleanerType.WindowsMemoryCleaner)
            {
                await LaunchWinMemoryCleaner(logIdent);
            }
        }

        private async Task LaunchMemReduct(string logIdent)
        {
            try
            {
                if (!File.Exists(MemReductExe))
                {
                    App.Logger.WriteLine(logIdent, "MemReduct not found, downloading portable version...");
                    SetStatus("Downloading MemReduct...");
                    await DownloadMemReduct();
                }

                if (!File.Exists(MemReductExe)) return;

                App.Logger.WriteLine(logIdent, "Launching MemReduct...");
                _memReductProcess = Process.Start(new ProcessStartInfo
                {
                    FileName = MemReductExe,
                    Arguments = "/minimized",
                    WorkingDirectory = MemReductDir,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
            }
            catch (Exception ex)
            {
                App.Logger.WriteLine(logIdent, "Failed to launch MemReduct: " + ex.Message);
            }
        }

        private async Task LaunchWinMemoryCleaner(string logIdent)
        {
            try
            {
                string cleanerDir = Path.Combine(Paths.Base, "WinMemoryCleaner");
                string cleanerExe = Path.Combine(cleanerDir, "WinMemoryCleaner.exe");

                if (!File.Exists(cleanerExe))
                {
                    App.Logger.WriteLine(logIdent, "Windows Memory Cleaner not found, downloading...");
                    SetStatus("Downloading Windows Memory Cleaner...");
                    Directory.CreateDirectory(cleanerDir);
                    using var http = new HttpClient();
                    http.DefaultRequestHeaders.UserAgent.ParseAdd("StarStrap/1.0");
                    var response = await http.GetAsync("https://github.com/IgorMundstein/WinMemoryCleaner/releases/latest/download/WinMemoryCleaner.exe");
                    response.EnsureSuccessStatusCode();
                    await using var fs = File.Create(cleanerExe);
                    await response.Content.CopyToAsync(fs);
                }

                if (!File.Exists(cleanerExe)) return;

                App.Logger.WriteLine(logIdent, "Launching Windows Memory Cleaner...");
                _memReductProcess = Process.Start(new ProcessStartInfo
                {
                    FileName = cleanerExe,
                    WorkingDirectory = cleanerDir,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Minimized
                });
            }
            catch (Exception ex)
            {
                App.Logger.WriteLine(logIdent, "Failed to launch Windows Memory Cleaner: " + ex.Message);
            }
        }

        private async Task DownloadMemReduct()
        {
            const string downloadUrl = "https://github.com/henrypp/memreduct/releases/latest/download/memreduct-3.4-bin.zip";
            try
            {
                Directory.CreateDirectory(MemReductDir);
                string zipPath = Path.Combine(MemReductDir, "memreduct.zip");
                using var http = new HttpClient();
                http.Timeout = TimeSpan.FromMinutes(5);
                http.DefaultRequestHeaders.UserAgent.ParseAdd("StarStrap/1.0");
                var response = await http.GetAsync(downloadUrl);
                response.EnsureSuccessStatusCode();
                await using var fs = File.Create(zipPath);
                await response.Content.CopyToAsync(fs);
                fs.Close();
                System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, MemReductDir, true);
                if (!File.Exists(MemReductExe))
                {
                    var found = Directory.GetFiles(MemReductDir, "memreduct.exe", SearchOption.AllDirectories).FirstOrDefault();
                    if (found != null && found != MemReductExe)
                    {
                        string subDir = Path.GetDirectoryName(found)!;
                        foreach (var file in Directory.GetFiles(subDir))
                        {
                            string destFile = Path.Combine(MemReductDir, Path.GetFileName(file));
                            if (!File.Exists(destFile)) File.Move(file, destFile);
                        }
                    }
                }
                if (File.Exists(zipPath)) File.Delete(zipPath);
            }
            catch (Exception ex)
            {
                App.Logger.WriteLine("MemReduct", "Failed to download MemReduct: " + ex.Message);
            }
        }

        private void StopCleaners()
        {
            try
            {
                if (_memReductProcess is not null && !_memReductProcess.HasExited)
                {
                    _memReductProcess.Kill();
                }
                _memReductProcess = null;

                foreach (var proc in Process.GetProcessesByName("memreduct")) { try { proc.Kill(); } catch { } }
                foreach (var proc in Process.GetProcessesByName("WinMemoryCleaner")) { try { proc.Kill(); } catch { } }
            }
            catch (Exception) { }
        }

        #endregion
