using System.Collections.Generic;

namespace HSM.WebApp.Models
{
    public class StatusModel<TModel> where TModel: class
    {
        public string ModelId { get; set; }
        public TModel Model { get; set; }
        public List<string> Errors { get; set; }
    }
}
