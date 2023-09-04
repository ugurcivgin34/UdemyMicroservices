namespace FreeCourse.Services.Order.Domain.Core
{
    public abstract class Entity
    {
        // Eşitlik kontrolü için kullanılan önbelleklenmiş hash kod değeri
        private int? _requestedHashCode;
        // Benzersiz kimlik değeri için özel alan
        private int _Id;

        // Id özelliği (property). Bu, entity'nin benzersiz tanımlayıcısıdır.
        public virtual int Id
        {
            get => _Id;
            set => _Id = value;
        }

        // Entity'nin geçici (transient) olup olmadığını kontrol eder. Eğer Id değeri atanmamışsa (default değerine sahipse) bu entity geçicidir.
        public bool IsTransient()
        {
            return this.Id == default(Int32);
        }

        // Bu sınıf için özelleştirilmiş GetHashCode metodunun override'ı
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                // Önbelleklenmiş hash kod değeri atanmamışsa yeni bir değer hesapla ve ata
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31;  // XOR işlemi rastgele dağılım sağlar

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }

        // Bu sınıf için özelleştirilmiş Equals metodunun override'ı
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Entity item = (Entity)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id == this.Id;
        }

        // == operatörünü özelleştiren metod
        public static bool operator ==(Entity left, Entity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        // != operatörünü özelleştiren metod
        public static bool operator !=(Entity left, Entity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? false : true;
            else
                return !left.Equals(right);
        }
    }

}
