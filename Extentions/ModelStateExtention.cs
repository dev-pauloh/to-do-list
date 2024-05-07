using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Todo.Extentions
{
    public static class ModelStateExtention
    {
        // Cria uma extensão para a classe nativa ModelState
        public static List<string> GetErrors(this ModelStateDictionary modelState) // Por padrão, os métodos de extensão devem ser estáticos
        {
            var result = new List<string>();
            foreach (var item in modelState.Values)
            {
                foreach (var error in item.Errors)
                {
                    result.Add(error.ErrorMessage);
                }
            }
            return result;
        }
    }
}
