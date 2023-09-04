namespace FreeCourse.Services.Order.Domain.Core
{
    public abstract class ValueObject
    {
        // İki değer nesnesinin (ValueObject) eşit olup olmadığını kontrol eden yardımcı metod.
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))  // Eğer sadece biri null ise false dön.
            {
                return false;
            }
            // İkisi de null ise veya left nesnesi right'a eşitse true dön.
            return ReferenceEquals(left, null) || left.Equals(right);
        }

        // İki değer nesnesinin farklı olup olmadığını kontrol eden yardımcı metod.
        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        // Eşitlik kontrolü için kullanılacak bileşenleri sağlar.
        protected abstract IEnumerable<object> GetEqualityComponents();

        // Override edilmiş Equals metodudur. Objeyi aldığı değer nesnesiyle karşılaştırır.
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())  // Eğer obje null veya tipi bu sınıfa eşit değilse false dön.
            {
                return false;
            }

            var other = (ValueObject)obj;

            // Bu nesnenin eşitlik bileşenleri ile diğer nesnenin eşitlik bileşenlerini karşılaştır.
            return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        // Override edilmiş GetHashCode metodudur. Nesnenin hash kodunu döndürür.
        public override int GetHashCode()
        {
            return GetEqualityComponents()
             .Select(x => x != null ? x.GetHashCode() : 0)  // Eğer bileşen null değilse hash kodunu al, değilse 0 kullan.
             .Aggregate((x, y) => x ^ y);  // XOR işlemi kullanarak toplam hash kodunu hesapla.
        }

        // Bu nesnenin bir kopyasını döndüren metod.
        public ValueObject? GetCopy()
        {
            // MemberwiseClone, mevcut nesnenin sığ bir kopyasını oluşturur.
            return this.MemberwiseClone() as ValueObject;
        }
    }

}
