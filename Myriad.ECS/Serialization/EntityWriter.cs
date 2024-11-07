//using System.Text.Json;
//using System.Text.Json.Serialization;

//namespace Myriad.ECS.Serialization;

//public class EntityWriter
//{
//    private readonly JsonSerializerOptions _options;

//    public EntityWriter(RelationMode relations = RelationMode.Throw)
//    {
//        _options = new JsonSerializerOptions()
//        {
//            NumberHandling = JsonNumberHandling.WriteAsString,
//        };
//        _options.Converters.Add(new EntityConverter(relations));
//    }

//    public void Serialize(Stream dest, Entity entity)
//    {
//        using var writer = new Utf8JsonWriter(dest, new JsonWriterOptions { Indented = true });

//        var remap = new Dictionary<EntityId, int>();
//        var done = new HashSet<EntityId>();
//        var todo = new Queue<EntityId>();

//        todo.Enqueue(entity.ID);

//        writer.WriteStartObject();
//        {
//            writer.WriteStartObject("entities");
//            {
//                while (todo.Count > 0)
//                {
//                    var item = todo.Dequeue();
//                    if (!done.Add(item))
//                        continue;

//                    var id = remap.Count + 1;
//                    remap.Add(item, id);

//                    writer.WriteStartObject(id.ToString());
//                    JsonSerializer.Serialize(writer, new EntityWithSerializationData(entity), _options);
//                    writer.WriteEndObject();
//                }
//            }
//            writer.WriteEndObject();
//        }
//        writer.WriteEndObject();
//    }

//    public enum RelationMode
//    {
//        /// <summary>
//        /// Set the target of relational components to null
//        /// </summary>
//        Null,

//        /// <summary>
//        /// Follow the links and also serialize the linked entity too
//        /// </summary>
//        Follow,

//        /// <summary>
//        /// Throw if there are any relational components
//        /// </summary>
//        Throw,
//    }

//    private record struct EntityWithSerializationData(Entity Entity);

//    private class EntityConverter
//        : JsonConverter<EntityWithSerializationData>
//    {
//        private readonly RelationMode _relations;

//        public EntityConverter(RelationMode relations)
//        {
//            _relations = relations;
//        }

//        public override EntityWithSerializationData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
//            throw new NotImplementedException();
//        }

//        public override void Write(Utf8JsonWriter writer, EntityWithSerializationData value, JsonSerializerOptions options)
//        {
//            if (_relations == EntityWriter.RelationMode.Throw)
//                foreach (var type in value.Entity.ComponentTypes)
//                    if (type.IsRelationComponent)
//                        throw new InvalidOperationException("EntityWriter.RelationMode is set to `Throw` - Cannot serialize entity with relational components");

//            writer.WriteStartObject("components");
//            {

//            }
//            writer.WriteEndObject();

//            //throw new NotImplementedException();
//        }
//    }
//}



