using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    class Crypter
    {
        public CrypterOptions Options { get; set; }
        public Crypter(CrypterOptions options)
        {
            this.Options = options;
        }
        public string Encrypt(string path)
        {
            
            var newPath = path.Replace(Options.SourcePath, Options.TargetPath);
            var dir = Path.GetDirectoryName(newPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            if (File.Exists(newPath))
                File.Delete(newPath);
            File.Copy(path, newPath);
            var text = File.ReadAllText(path);
            using(var aes = Aes.Create())
            {
                aes.Key = Options.Key;
                aes.IV = Options.Key;
                var encryptor = aes.CreateEncryptor();
                using (var ms = new MemoryStream())
                {
                    using(var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(text);
                        }
                        File.WriteAllBytes(newPath, ms.ToArray());
                    }
                }
            }
            
            Options.TargetLogger.RecordEntry($"Файл {Path.GetFileName(path)} зашифрован");
            return newPath;
        }

        public string Decrypt(string path)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Options.Key;
                aes.IV = Options.Key;
                var decryptor = aes.CreateDecryptor();
                using (var ms = new MemoryStream(File.ReadAllBytes(path)))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            File.WriteAllText(path, sr.ReadToEnd());
                        }
                    }
                }
            }
            Options.TargetLogger.RecordEntry($"Файл {Path.GetFileName(path)} расшифрован");
            return path;
        }
    }
}
