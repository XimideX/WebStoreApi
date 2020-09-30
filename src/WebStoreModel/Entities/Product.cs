using WebStoreModel.CoreModel;

namespace WebStoreModel.Entities
{
    public class Product : BaseModel
    {
        public string Name { get; set; }
        public float Value { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Codigo { get; set; }

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
            throw new System.NotImplementedException();
        }
    }
}