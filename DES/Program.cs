// copyright merzlovnik@mail.ru github.com/volpertinger
using DES;
using System.Reflection;
using System.Text.Json;
Console.WriteLine(@"
██████╗ ███████╗███████╗                                                                         
██╔══██╗██╔════╝██╔════╝                                                                         
██║  ██║█████╗  ███████╗                                                                         
██║  ██║██╔══╝  ╚════██║                                                                         
██████╔╝███████╗███████║                                                                         
╚═════╝ ╚══════╝╚══════╝                                                                         
██████╗ ██╗   ██╗                                                                                
██╔══██╗╚██╗ ██╔╝                                                                                
██████╔╝ ╚████╔╝                                                                                 
██╔══██╗  ╚██╔╝                                                                                  
██████╔╝   ██║                                                                                   
╚═════╝    ╚═╝                                                                                   
██╗   ██╗ ██████╗ ██╗     ██████╗ ███████╗██████╗ ████████╗██╗███╗   ██╗ ██████╗ ███████╗██████╗ 
██║   ██║██╔═══██╗██║     ██╔══██╗██╔════╝██╔══██╗╚══██╔══╝██║████╗  ██║██╔════╝ ██╔════╝██╔══██╗
██║   ██║██║   ██║██║     ██████╔╝█████╗  ██████╔╝   ██║   ██║██╔██╗ ██║██║  ███╗█████╗  ██████╔╝
╚██╗ ██╔╝██║   ██║██║     ██╔═══╝ ██╔══╝  ██╔══██╗   ██║   ██║██║╚██╗██║██║   ██║██╔══╝  ██╔══██╗
 ╚████╔╝ ╚██████╔╝███████╗██║     ███████╗██║  ██║   ██║   ██║██║ ╚████║╚██████╔╝███████╗██║  ██║
  ╚═══╝   ╚═════╝ ╚══════╝╚═╝     ╚══════╝╚═╝  ╚═╝   ╚═╝   ╚═╝╚═╝  ╚═══╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝
                                                                                                                                                                                                                                                           
");

string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", @"Settings.json");
string jsonString = File.ReadAllText(path);
Settings? settings = JsonSerializer.Deserialize<Settings>(jsonString);

if (settings is null)
    throw new ArgumentException(String.Format("Invalid Settings.json file\n{0}", jsonString));

var des = new DES.DES.DES(settings.Key);

foreach (var setting in settings.Operations)
{
    if (File.Exists(setting.PathOutput))
    {
        throw new ArgumentException(String.Format("File with path {0} Already exists!", setting.PathInput));
    }

    using (FileStream fsi = File.OpenRead(setting.PathInput))
    {
        using (FileStream fso = File.OpenWrite(setting.PathOutput))
        {
            switch (setting.Operation)
            {
                case Operations.Encrypt:
                    des.Encrypt(fsi, fso);
                    break;
                case Operations.Decrypt:
                    des.Decrypt(fsi, fso);
                    break;
                default:
                    throw new ArgumentException("Something went wrong. Better pray.");
            };
            fso.Close();
        }
        fsi.Close();
    }
}
