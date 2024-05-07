using System.ComponentModel.DataAnnotations;

namespace Todo.ViewModels.Todos
{
    public class EditorTodoViewModel
    {
        [Required(ErrorMessage = "O titulo é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Este campo deve conter entre 3 e 40 caracteres")]
        public string Title { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório")]
        public bool Done { get; set; }
    }
}
