using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RlViewer.Behaviors.Converters;

namespace RlViewer.Behaviors.Navigation.NavigationChanger
{
    public class Brl4NavigationChanger
    {
        public Brl4NavigationChanger(string fileName)
        {
            _brlFileName = fileName;
            var prop = new Files.FileProperties(fileName);

            _header = Factories.Header.Abstract.HeaderFactory.GetFactory(prop).Create(fileName) as Headers.Concrete.Brl4.Brl4Header;
            _brl4File = Factories.File.Abstract.FileFactory.GetFactory(prop).Create(prop, _header, null) as Files.Rli.Concrete.Brl4;
        }

        private string _brlFileName;
        private Headers.Concrete.Brl4.Brl4Header _header;
        private Files.Rli.Concrete.Brl4 _brl4File;

        /// <summary>
        /// Checks if RLI has .ba RHG as its source
        /// </summary>
        /// <returns></returns>
        public bool CheckIsBaRhg()
        {
            if(_brl4File == null)
            {
                return false;
            }

            var rhgName = _brl4File.Header.HeaderInfo.Where(x => x.HeaderName == "РГГ")
                .FirstOrDefault().Params.Where(x => x.Item1 == "Имя файла").FirstOrDefault().Item2;

            return Path.GetExtension(rhgName) == ".ba";
        }


        /// <summary>
        /// Changes selected brl4 rli navigation based on .ba rhg file
        /// </summary>
        /// <param name="baFilename">Path to .ba source rhg</param>
        public void ChangeNavigation(string baFilename)
        {   
            if (_brl4File != null)
            {
                var baHeaderLength = 512;//header for .ba files is 512 bytes
                var offsetToNavigation = 48;
                var strDataLength = 32768 - 512;

                var headerData = new byte[baHeaderLength];


                using (var fr = File.Open(baFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var fw = File.Open(_brl4File.Properties.FilePath, FileMode.Open, FileAccess.Write, FileShare.Write))
                    {
                        fw.Seek(_brl4File.Header.FileHeaderLength, SeekOrigin.Current);

                        for (int i = 0; i < _header.HeaderStruct.rlParams.sy; i++)
                        {
                            fr.Seek(baHeaderLength + strDataLength, SeekOrigin.Current);
                        }

                        for (int i = 0; i < _brl4File.Height; i++)
                        {
                            fr.Seek(offsetToNavigation, SeekOrigin.Current);
                            var brl4StrHeader = StructIO.ReadStruct<Headers.Concrete.Ba.BaStrHeader>(fr).ToBrl4StrHeader();
                            fr.Seek(baHeaderLength - offsetToNavigation - 
                                System.Runtime.InteropServices.Marshal.SizeOf(new Headers.Concrete.Ba.BaStrHeader()), SeekOrigin.Current);                           
                            fr.Seek(strDataLength, SeekOrigin.Current);

                            var brl4HeaderBytes = StructIO.WriteStruct<Headers.Concrete.Brl4.Brl4StrHeaderStruct>(brl4StrHeader);

                            fw.Write(brl4HeaderBytes, 0, brl4HeaderBytes.Length);
                            fw.Seek(_brl4File.Width * _brl4File.Header.BytesPerSample, SeekOrigin.Current);
                        }                
                    }
                }


            }
        }


    }
}
