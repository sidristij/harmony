using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Mono.Cecil;

using Harmony.Sdk;

namespace Harmony.Aspect.TypesLevel
{
    public class TypeLevelAspect : AspectBase
    {

        Dictionary<string, IMemberDefinition> _exports = new Dictionary<string, IMemberDefinition>();

        protected virtual void LinkExports()
        {
            /*
            var thisType = GetType();

            // Collecting exported fields
            foreach(var member in thisType.GetMembers())
            {
                var attrs = member.GetCustomAttributes().OfType<ExportMemberAttribute>();
                var count = attrs.Count();

                if(count > 1) 
                {
                    // Log error
                } else if(count == 0)
                {
                    continue;
                }

                // var exportedField = DuplicateFieldDefinitionAdvice.Run(member.Name, TypeDef.Of(GetType()), FieldRef.Of(member));
                // _exports.Add(member.Name, exportedField);
            }
            */
            // Collecting exported methods
            // Collecting exported properties
            // Collecting exported events
        }
        
        public IEnumerable<FieldDefinition> ExportedFields {
            get {
                return _exports.Where(kv => kv.Value is FieldDefinition).Select(field => field.Value as FieldDefinition);
            }
        }

        public IEnumerable<MethodDefinition> ExportedMethods {
            get {
                return _exports.Where(kv => kv.Value is MethodDefinition).Select(method => method.Value as MethodDefinition);
            }
        }

        public IEnumerable<PropertyDefinition> ExportedProperties {
            get {
                return _exports.Where(kv => kv.Value is PropertyDefinition).Select(prop => prop.Value as PropertyDefinition);
            }
        }

        public IEnumerable<EventDefinition> ExportedEvents {
            get {
                return _exports.Where(kv => kv.Value is EventDefinition).Select(@event => @event.Value as EventDefinition);
            }
        }
    }
}

