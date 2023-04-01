// copyright merzlovnik@mail.ru github.com/volpertinger

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

string pathI = @"C:\Users\MerzlovNikolay\source\repos\DES\DES\Examples\Plain\Text.txt";
string pathOC = @"C:\Users\MerzlovNikolay\source\repos\DES\DES\Examples\Encrypted\Text.txt";
string pathOD = @"C:\Users\MerzlovNikolay\source\repos\DES\DES\Examples\Decrypted\Text.txt";

var ss = new DES.DES.DES(128);

if (File.Exists(pathOD))
{
    File.Delete(pathOD);
}

using (FileStream fsi = File.OpenRead(pathI))
{
    // Delete files if it exists.
    if (File.Exists(pathOC))
    {
        File.Delete(pathOC);
    }
    using (FileStream fso = File.OpenWrite(pathOC))
    {
        ss.Encrypt(fsi, fso);
        fso.Close();
    }
    fsi.Close();
}

using (FileStream fsi = File.OpenRead(pathOC))
{
    // Delete files if it exists.
    if (File.Exists(pathOD))
    {
        File.Delete(pathOD);
    }
    using (FileStream fso = File.OpenWrite(pathOD))
    {
        ss.Decrypt(fsi, fso);
        fso.Close();
    }
    fsi.Close();
}
