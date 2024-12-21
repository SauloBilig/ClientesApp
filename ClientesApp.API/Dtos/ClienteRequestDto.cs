using System.ComponentModel.DataAnnotations;

namespace ClientesApp.API.Dtos
{
    /// <summary>
    /// Objeto para modelagem dos dados que a API deverá receber
    /// para realizar o cadastro ou edição de um cliente
    /// </summary>
    public class ClienteRequestDto
    {
        #region Propriedades
        [MinLength(8, ErrorMessage ="Por favor, informe no minimo {1} caracteres.")]
        [MaxLength(150, ErrorMessage = "Por favor, informe no maximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o Nome do cliente.")]
        public string Nome { get; set; }

        [EmailAddress(ErrorMessage ="Por favor, informe um endereço de Email valido.")]
        [Required(ErrorMessage = "Por favor, informe o Email do cliente.")]
        public string Email { get; set; }

        [RegularExpression(@"^\d{11}$", ErrorMessage = "Por favor, informe o cpf com apenas 11 números.")]
        [Required(ErrorMessage = "Por favor, informe o CPF do cliente.")]
        public string Cpf { get; set; }

        [Range(1,4, ErrorMessage = "Por favor, informe uma Categoria de 1 a 4.")]
        [Required(ErrorMessage = "Por favor, informe a Categoria do cliente.")]
        public int Categoria { get; set; }

        #endregion
    }
}
