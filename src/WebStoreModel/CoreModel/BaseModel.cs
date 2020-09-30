namespace WebStoreModel.CoreModel
{
    public abstract class BaseModel
    {
        public int Id { get; set; }

        public override abstract string ToString();

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is BaseModel))
            {
                return false;
            }
            BaseModel m = (BaseModel)obj;
            return m.Id == this.Id;
        }

        public override int GetHashCode()
        {
            if (this.Id > 0) return this.Id;
            return base.GetHashCode();
        }
    }
}