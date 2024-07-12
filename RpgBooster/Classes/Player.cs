using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgBooster.Classes
{
    internal class Player
    {
        //Public pois vai ser usado nos numeric e textbox
        public string Nome;
        public string Classe;
        public decimal Nivel;
        public decimal Exp;
        public decimal Vida;
        public decimal Força;
        public decimal Persistência;
        public decimal Sorte;
        public decimal Agilidade;
        public decimal Carisma;
        public decimal Inteligência;
        public string Inventario;

        public Player(string nome, string classe, decimal nivel, decimal exp, decimal força, decimal persistencia, decimal sorte, decimal agilidade, decimal carisma, decimal inteligência, string inventario)
        {
            Nome = nome;
            Classe = classe;
            Nivel = nivel;
            Exp = exp;
            Vida = 20 + persistencia;
            Força = força;
            Persistência = persistencia;
            Sorte = sorte;
            Agilidade = agilidade;
            Carisma = carisma;
            Inteligência = inteligência;
            Inventario = inventario;
        }
    }
}
