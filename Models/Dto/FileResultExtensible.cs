namespace ANTU.Models.Dto
{
    public partial class FileResultExtensible : FileBase
    {
        public FileResultExtensible(FileBase file) : base(file)
        {
        }

        public string codigo { set; get; } = Guid.NewGuid().ToString();
        
       
    }
}
