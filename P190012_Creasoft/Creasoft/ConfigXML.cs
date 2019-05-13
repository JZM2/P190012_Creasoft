using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace P190012_Creasoft.Creasoft
{
    class ConfigXML
    {
        public string ConfigFile { set; get; }
        public List<Folder> ImportFolders;

        public static Creasoft.ConfigXML Create(string FilePath)
        {
            return new ConfigXML(FilePath);
        }

        public ConfigXML()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilePath"></param>
        public ConfigXML(string FilePath)
        {
            string _file = "";

            this.ImportFolders = new List<Folder>();

            XmlDocument Doc = new XmlDocument();
            Doc.PreserveWhitespace = true;

            try
            {
                Doc.Load(FilePath);

                XmlNode Root = Doc.DocumentElement;
                foreach (XmlNode Node in Root.ChildNodes)
                {
                    if (Node.Name.Equals("FOLDER"))
                    {
                        Folder fld = new Folder();
                        XmlAttributeCollection Atts = Node.Attributes;
                        foreach (XmlAttribute Attribute in Atts)
                        {
                            switch (Attribute.Name)
                            {
                                case "archive":
                                    fld.Archive = Attribute.Value;
                                    break;
                                case "output":
                                    fld.Output = Attribute.Value;
                                    break;
                                case "filter":
                                    fld.Filter = Attribute.Value;
                                    break;
                                case "path":
                                    fld.Path = Attribute.Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                        this.ImportFolders.Add(fld);
                    }                   
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine(String.Format("Soubor \"{0}\" nebyl nalezen!", _file));
            }
            finally
            {
                this.ConfigFile = FilePath;
            }

            
        }

        /// <summary>
        /// Spouští zpracování údajů v konfiguračním souboru
        /// </summary>
        public void Execute()
        {
            int i = 1;
            foreach (Creasoft.Folder folder in ImportFolders)
            {
                Console.WriteLine("{4}) složka:  \"{1}\", archivace: \"{2}\", výstup: \"{3}\", soubory: \"{5}\"", 0, folder.Path, folder.Archive, folder.Output, i, folder.Filter);
                folder.ExecuteFiles();

                i++;
            }
        }

    }
}
