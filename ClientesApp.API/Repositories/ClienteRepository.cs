using ClientesApp.API.Dtos;
using ClientesApp.API.Entities;
using Dapper;
using System.Data.SqlClient;

namespace ClientesApp.API.Repositories
{
    /// <summary>
    /// Repositório para gravar, alterar, excluir e consultar
    /// dados de clientes no banco de dados.
    /// </summary>
    public class ClienteRepository
    {
        //atributo para armazenar a string de conexão do banco de dados
        private string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ClientesApp;Integrated Security=True;";

        /// <summary>
        /// Método para inserir um registro de cliente
        /// na tabela do banco de dados do SqlServer
        /// </summary>
        public void Inserir(Cliente cliente)
        {
            //abrir conexão com o banco de dados
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    INSERT INTO CLIENTE(ID, NOME, EMAIL, CPF, DATACRIACAO, DATAULTIMAALTERACAO, ATIVO, CATEGORIA)
                    VALUES(@ID, @NOME, @EMAIL, @CPF, @DATACRIACAO, @DATAULTIMAALTERACAO, @ATIVO, @CATEGORIA)
                ";

                connection.Execute(query, new
                {
                    @ID = cliente.Id,
                    @NOME = cliente.Nome,
                    @EMAIL = cliente.Email,
                    @CPF = cliente.Cpf,
                    @DATACRIACAO = cliente.DataCriacao,
                    @DATAULTIMAALTERACAO = cliente.DataUltimaAlteracao,
                    @ATIVO = cliente.Ativo,
                    @CATEGORIA = (int) cliente.Categoria
                });
            }
        }

        public void Atualizar(Cliente cliente)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    UPDATE C SET  C.Nome=@NOME,
                                  C.Email=@EMAIL,
                                  C.Cpf=@CPF,
                                  C.Categoria = @CATEGORIA,
                                  C.DataUltimaAlteracao = @DATAULTIMAALTERACAO
                    FROM CLIENTE C
                    WHERE Id = @ID

                ";
                connection.Execute(query, new
                {
                    @ID = cliente.Id,
                    @NOME = cliente.Nome,
                    @EMAIL = cliente.Email,
                    @CPF = cliente.Cpf,
                    @DATAULTIMAALTERACAO = DateTime.Now,
                    @CATEGORIA = (int) cliente.Categoria
                });
            }
        }

        public void Excluir (Guid Id) 
        { 
        
            using(var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    UPDATE C SET  C.Ativo=0,
                                  C.DataUltimaAlteracao = @DATAULTIMAALTERACAO
                    FROM CLIENTE C
                    WHERE Id = @ID

                ";
                connection.Execute(query, new
                {
                    @ID = Id,
                    @DATAULTIMAALTERACAO = DateTime.Now
                });  
            }
        
        }

        public List<Cliente> Consultar()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    SELECT * 
                    FROM CLIENTE 
                    WHERE ATIVO =  1 
                    ORDER BY NOME 

                    WAITFOR DELAY '00:00:10';
                ";
                return connection.Query<Cliente>(query).ToList(); 
            }
        }

        public Cliente? ObterPorId (Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    SELECT * 
                    FROM CLIENTE 
                    WHERE ATIVO =  1 and id = @ID
                    ORDER BY NOME 
                ";
                return connection
                   .Query<Cliente>(query, new { @ID = id })
                   .FirstOrDefault();

            }
        }

        public List<ClienteDashboardDto> ConsultarDashboard()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    WAITFOR DELAY '00:00:05';

                    SELECT
	                    CASE
		                    WHEN CATEGORIA = 1 THEN 'CLIENTE COMUM'
		                    WHEN CATEGORIA = 2 THEN 'CLIENTE PREFERENCIAL'
		                    WHEN CATEGORIA = 3 THEN 'CLIENTE EMPRESA'
		                    WHEN CATEGORIA = 4 THEN 'CLIENTE VIP'
                        END AS 'NOMECATEGORIA',
	                    COUNT(*) AS 'QUANTIDADECLIENTES'
                        FROM CLIENTE
                        WHERE ATIVO = 1
                        GROUP BY CATEGORIA
                        ORDER BY QUANTIDADECLIENTES DESC

                ";
                return connection.Query<ClienteDashboardDto>(query).ToList();
            }
        }


    }
    
}
