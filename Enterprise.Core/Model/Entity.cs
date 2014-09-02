namespace Enterprise.Core.Model
{
    public abstract class Entity<T>
    {
        public virtual T Id { get; set; }
    }
}
