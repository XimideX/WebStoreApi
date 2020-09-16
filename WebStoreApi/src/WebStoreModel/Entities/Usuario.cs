using WebStoreModel.CoreModel;

namespace WebStoreModel.Entities
{
    public class Usuario : BaseModel
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}