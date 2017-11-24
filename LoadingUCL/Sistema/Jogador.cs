using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoadingUCL.Sistema
{
    public class Jogador
    {
        public string Nome { get; set; }
        public bool Desenhando { get; set; }
        public double Pontuacao { get; set; }
        public bool PontuacaoVerificada { get; set; } = false;
    }
}