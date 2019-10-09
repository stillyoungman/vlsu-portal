using System.ComponentModel;

namespace Portal.Models
{
    //пришлось продублировать атрибуты в интерфейсе, т.к. атрибуты сущности под интерфесом не видны

    public interface IPlanTitle
    {
        string Name { get; }
        [DisplayName("Шифр")]
        string Code { get; }
        string Degree { get; }
        [DisplayName("Наименование направления (специальности)")]
        string Full { get; }
        string Short { get; }
        string Term { get; }
    }
}