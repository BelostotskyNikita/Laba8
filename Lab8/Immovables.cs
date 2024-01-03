using System.Runtime.Serialization;
namespace Lab8
{
    public class Immovables
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string? Name { get; set; }
        [DataMember]
        public long PropertyValue { get; set; }
        public Immovables() { }
        public Immovables(int id) => (Id, Name, PropertyValue) = (id, LoremNET.Lorem.Words(1, 1), LoremNET.Lorem.Number(0, 100000));
    }
}
