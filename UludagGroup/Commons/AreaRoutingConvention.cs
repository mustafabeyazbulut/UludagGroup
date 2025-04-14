using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace UludagGroup.Commons
{
    internal sealed class AreaRoutingConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var hasRouteAttributes = controller.Selectors.Any(selector => selector.AttributeRouteModel != null);

            if (hasRouteAttributes)
            {
                return;
            }

            var controllerTypeNamespace = controller.ControllerType.Namespace;
            if (controllerTypeNamespace == null)
                return;

            var areaName = controllerTypeNamespace.Split('.').SkipWhile(segment => segment != "Areas").Skip(1).FirstOrDefault();


            if (!string.IsNullOrEmpty(areaName))
            {
                var template = areaName + "/[controller]/[action]/{id?}";
                controller.RouteValues.Add("area", areaName);

                foreach (var selector in controller.Selectors)
                {
                    selector.AttributeRouteModel = new AttributeRouteModel
                    {
                        Template = template
                    };
                }
            }
        }
    }
}
