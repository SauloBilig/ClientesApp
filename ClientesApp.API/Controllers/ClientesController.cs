using ClientesApp.API.Dtos;
using ClientesApp.API.Entities;
using ClientesApp.API.Enums;
using ClientesApp.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClientesApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        /// <summary>
        /// Serviço para cadastro de cliente na API
        /// </summary>
        [HttpPost]
        public IActionResult Post(ClienteRequestDto dto)
        {
            //instanciando um objeto da classe Cliente
            var cliente = new Cliente();

            //capturando os dados da requisição
            cliente.Id = Guid.NewGuid();
            cliente.Nome = dto.Nome;
            cliente.Email = dto.Email;
            cliente.Cpf = dto.Cpf;
            cliente.DataCriacao = DateTime.Now;
            cliente.DataUltimaAlteracao = DateTime.Now;
            cliente.Ativo = true;
            cliente.Categoria = (Categoria)dto.Categoria;

            //cadastrando o cliente no banco de dados
            var clienteRepository = new ClienteRepository();
            clienteRepository.Inserir(cliente);

            return Ok(new { mensagem = "Cliente cadastrado com sucesso!" });
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, ClienteRequestDto dto)
        {
            //instanciando um objeto da classe Cliente
            var cliente = new Cliente();

            //capturando os dados da requisição
            cliente.Id = id;
            cliente.Nome = dto.Nome;
            cliente.Email = dto.Email;
            cliente.Cpf = dto.Cpf;
            cliente.Categoria = (Categoria) dto.Categoria;

            var clienteRepository = new ClienteRepository();
            clienteRepository.Atualizar(cliente);

            return Ok(new { mensagem = "Cliente atualizado com sucesso!" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var clienteRepository = new ClienteRepository();
            clienteRepository.Excluir(id);

            return Ok(new { mensagem = "Cliente excluido com sucesso!" });
        }

        [HttpGet]
        public IActionResult Get()
        {
            var clienteRepository = new ClienteRepository();
            var clientes = clienteRepository.Consultar();

            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var clienteRepository = new ClienteRepository();
            var cliente = clienteRepository.ObterPorId(id);

            return Ok(cliente);
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult GetDashboard()
        {
            var clienteRepository = new ClienteRepository();
            var dashboard = clienteRepository.ConsultarDashboard();

            return Ok(dashboard);
        }
    }
}
