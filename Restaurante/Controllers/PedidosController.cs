using Microsoft.AspNetCore.Mvc;
using Restaurante.Entidades;

namespace Restaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : Controller
    {
        //Cria lista para armazenar os pedidos, como um banco de dados
        public static List<Pedido> ArmazenarPedidos = new List<Pedido>();

        //Endpoint para criar pedido, que recebe um item
        [HttpPost("CriaPedido")]
        public IActionResult CriaPedido(string item)
        {
            try
            {
                //Novo pedido é criado
                Pedido pedido = new Pedido();
                //O pedido tem um item atribuído, no caso o item passado via parâmetro
                pedido.Item = item;
                //O STATUS do pedido é que ele foi solicitado
                pedido.StatusPedido = Enumeradores.StatusPedidoEnum.SOLICITADO;
                //O número dele é a quantidade de pedidos +1
                pedido.NumeroPedido = ArmazenarPedidos.Count() + 1;
                //Pedido é adicionado na lista de pedidos
                ArmazenarPedidos.Add(pedido);
                //Retornamos um código de sucesso, com o objeto do pedido
                return Ok(pedido);
            }
            catch (Exception e)
            {
                //Caso dê algum problema retornamos a mensagem de erro
                return StatusCode(500, e.Message);
            }
        }

        //Método que recebe um número de pedido
        [HttpPost("ProcessarPedido")]
        public IActionResult ProcessarPedido(int numeroDoPedido)
        {
            try
            {
                // É feita uma busca na lista de pedidos pelo número de pedido passado via parâmetro
                Pedido? pedidoEncontrado = ArmazenarPedidos.
                     Find(pedido => pedido.NumeroPedido == numeroDoPedido);
                //verifica se o pedido foi encontrado, se não foi, ele será nulo
                if (pedidoEncontrado == null)
                {
                    //se não for encontrado nenhum pedido com o número passado
                    //Retornamos um código de erro com uma mensagem de erro
                    return BadRequest("Não foi encontrado nenhum pedido com esse número!");
                }
                else
                {
                    //Caso o pedido seja encontrado o status dele muda para EM PREPARACAO
                    pedidoEncontrado.StatusPedido = Enumeradores.StatusPedidoEnum.EM_PREPARACAO;
                    //Devolvemos um código de sucesso com a mensagem: Pedido em Preparo!
                    return Ok("Pedido em preparo!");
                }

            }
            catch (Exception e)
            {
                //Caso ocorra uma exceção, retornamos um código de erro com a mensagem da exceção
                return StatusCode(500, e.Message);
            }
        }

        //Método que recebe um número de pedido
        [HttpPut("ConcluirPedido")]
        public IActionResult ConcluirPedido(int numeroPedido)
        {
            try
            {
                //Procura o número do pedido passado pelo parâmetro na lista de pedidos
                Pedido? pedidoEncontrado = ArmazenarPedidos.
                    Find(pedido => pedido.NumeroPedido == numeroPedido);
                //Verifica se encontrou o pedido
                if (pedidoEncontrado == null)
                {
                    //Se não encontrar retorna um código de erro, com uma mensagem de erro
                    return BadRequest("Não foi encontrado nenhum pedido com esse número!");
                }
                else
                {
                    //Se encontrar, altera o status para CONCLUIDO
                    pedidoEncontrado.StatusPedido = Enumeradores.StatusPedidoEnum.CONCLUIDO;
                    //Retorna um código de sucesso, com a mensagem: Pedido Concluído!
                    return Ok("Pedido Concluído!");
                }
            }
            catch (Exception ex)
            {
                //Caso ocorra uma exceção, retornamos um código de erro com a mensagem da exceção
                return StatusCode(500, ex.Message);
            }
        }

        //Método que não recebe parâmetros
        [HttpGet("ListaPedidos")]
        public IActionResult ListaPedidos()
        {
            try
            {
                //Retorna um código de sucesso com a lista de pedidos
                return StatusCode(200, ArmazenarPedidos);
            }
            catch (Exception ex)
            {
                //Caso ocorra uma exceção, retornamos um código de erro com a mensagem da exceção
                return StatusCode(500, ex.Message);
            }
        }

        //Método para apagar pedido que recebe um número de pedido
        [HttpDelete("ApagarPedido")]
        public IActionResult ApagarPedido(int numeroPedido)
        {
            try
            {
                // Cria uma variável do tipo pedido para armazenar o pedidoEncontrado
                Pedido? pedidoEncontrado = null; 
                //Verifica os itens na lista de pedidod
                foreach (Pedido pedido in ArmazenarPedidos)
                {
                    //Verifica se o pedido que está sendo visto na lista tem o
                    //mesmo número de pedido passado no parâmetro
                    if(pedido.NumeroPedido == numeroPedido) 
                    {
                        //Se tiver, atribui a variável pedidoEncontrado, que vira um objeto
                        pedidoEncontrado = pedido;
                    }
                }
                //Se encontrou um pedido com aquele número, ele remove o pedido da lista
                if (pedidoEncontrado != null)
                {
                    ArmazenarPedidos.Remove(pedidoEncontrado);
                    return StatusCode(200, "Pedido excluído!");
                }
                else
                {
                    return StatusCode(400, "Número do pedido não foi encontrado!");
                }

            }
            catch (Exception ex)
            {
                //Caso ocorra uma exceção, retornamos um código de erro com a mensagem da exceção
                return StatusCode(500, ex.Message);
            }
        }
    }
}