using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GetRptDefFile
{
    class Program
    {

        static string _scanPath; //Repertoire de recherche
        static string _ouputReportFile; // Repertoire ou le rappor sera créé

        static void Main(string[] args)
        {
            try
            {

                #region validation

                //Parametre sur la ligne de commande est obligatoire
                if (args.Length < 2)
                {                    
                    throw new ArgumentException(Resource1.ERR_ARG);
                }

                if (!Directory.Exists(args[0]))
                {
                    throw new System.IO.DirectoryNotFoundException(Resource1.ERR_NO_SOURCE_PATH);
                }

                if (!Directory.Exists(args[1]))
                {
                    throw new System.IO.DirectoryNotFoundException(Resource1.ERR_NO_DEST_PATH);
                }

                #endregion

                _scanPath = args[0];
                _ouputReportFile = args[1] + ((args[1].EndsWith("\\")) ? "" : "\\") + "report.csv";

                //Filtre pour les fichier rpt dans le répertoire et les sous-répertoires
                string[] filePaths = Directory.GetFiles(_scanPath, "*.rpt", SearchOption.AllDirectories);


                // Delete the file if it exists.
                if (File.Exists(_ouputReportFile))
                {
                    File.Delete(_ouputReportFile);
                }

                //Creation du fichier de rapport
                using (TextWriter tw = new StreamWriter(_ouputReportFile))
                {
                    tw.WriteLine("Create on : " + DateTime.Now);
                    tw.WriteLine("FileName;Version");

                    using (CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument())
                    {
                        //pour chaque fichier trouvé on lit la version du rapport et on l'inscrit dans le fichier
                        foreach (string file in filePaths)
                        {
                            //CrystalDecisions.Shared.ExportOptions t;
                            //t.ExportFormatType = (CrystalDecisions.Shared.ExportFormatType)34;

                            rpt.Load(file);

                            int majVer = rpt.ReportClientDocument.MajorVersion;
                            int minVer = rpt.ReportClientDocument.MinorVersion;
                            tw.WriteLine(file + ';' + majVer + '.' + minVer); //inscription au fichier
                            rpt.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Console.WriteLine("ERREUR : " + ex.Message);
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Console.WriteLine("");
            }
        }
    }
}
