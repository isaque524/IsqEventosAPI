using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace IsqEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }

        public string Local { get; set; }

        public string DataEvento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigtório."),
         MinLength(3, ErrorMessage = "{0} deve ter no mínimo 3 caracteres."),
         MaxLength(50, ErrorMessage = "{0} deve ter no máximo 50 caracteres.")
      ]
        public string Tema { get; set; }

        [Display(Name = "Qtd Pessoas")]
        [Range(1, 120000, ErrorMessage = "{0} Não pode ser menor que 1 e maior que 120.000")]
        public int QtdPessoas { get; set; }

        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$",
                                   ErrorMessage = "Não é uma imagem válida. (gif, jpg, jpeg, bmp ou png)")]
        public string ImagemURL { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigtório.")]
        [Phone(ErrorMessage = "O campo {0} está com o numero invalido!")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "e-mail")]
        [EmailAddress(ErrorMessage = "É necessário ser um {0} válido")]
        public string Email { get; set; }

        public IEnumerable<LoteDto>? Lotes { get; set; }

        public IEnumerable<RedeSocialDto>? RedesSociais { get; set; }

        public IEnumerable<PalestranteDto>? Palestrantes { get; set; }

    }
}