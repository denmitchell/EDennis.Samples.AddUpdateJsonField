using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.Samples.AddUpdateJsonField.Lib {

    public class JsonFieldAdderUpdater {

        public static async Task<string> AddUpdateFieldAsync<T>(string jsonSource, string fieldName, T value) {

            StringBuilder sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            using (JsonWriter writer = new JsonTextWriter(sw))
            using (var sr = new StringReader(jsonSource))
            using (JsonReader reader = new JsonTextReader(sr)) {
                writer.Formatting = Formatting.Indented;

                bool currentObjectHasTargetField = false;
                while (await reader.ReadAsync()) {
                    if (reader.TokenType == JsonToken.EndObject &&
                        currentObjectHasTargetField == false) {
                        await writer.WritePropertyNameAsync(fieldName);
                        await writer.WriteValueAsync(value);
                        await writer.WriteEndObjectAsync();
                        currentObjectHasTargetField = false; //reinitialize for next object
                    } else if (reader.TokenType == JsonToken.PropertyName &&
                        reader.Value.ToString() == fieldName) {
                        await writer.WritePropertyNameAsync(fieldName);
                        await writer.WriteValueAsync(value);
                        currentObjectHasTargetField = true; //to not add target field at end of object
                    } else {
                        switch (reader.TokenType) {
                            case JsonToken.PropertyName:
                                await writer.WritePropertyNameAsync(reader.Value.ToString());
                                break;
                            case JsonToken.Boolean:
                            case JsonToken.Bytes:
                            case JsonToken.Date:
                            case JsonToken.Float:
                            case JsonToken.Integer:
                            case JsonToken.String:
                                await writer.WriteValueAsync(reader.Value);
                                break;
                            case JsonToken.Null:
                                await writer.WriteNullAsync();
                                break;
                            case JsonToken.StartObject:
                                await writer.WriteStartObjectAsync();
                                break;
                            case JsonToken.StartArray:
                                await writer.WriteStartArrayAsync();
                                break;
                            case JsonToken.EndArray:
                                await writer.WriteEndArrayAsync();
                                break;
                            case JsonToken.Comment:
                                await writer.WriteCommentAsync(reader.Value.ToString());
                                break;
                        }
                    }
                }
            }

            return sb.ToString();
        }


        public static async Task AddUpdateFieldAsync<T>(Stream outputStream, Stream inputStream, string fieldName, T value) {

            var sw = new StreamWriter(outputStream);
            sw.AutoFlush = true;
            JsonWriter writer = new JsonTextWriter(sw);
            using (var sr = new StreamReader(inputStream))
            using (JsonReader reader = new JsonTextReader(sr)) {
                writer.Formatting = Formatting.Indented;

                bool currentObjectHasTargetField = false;
                while (await reader.ReadAsync()) {
                    if (reader.TokenType == JsonToken.EndObject &&
                        currentObjectHasTargetField == false) {
                        await writer.WritePropertyNameAsync(fieldName);
                        await writer.WriteValueAsync(value);
                        await writer.WriteEndObjectAsync();
                        currentObjectHasTargetField = false; //reinitialize for next object
                    } else if (reader.TokenType == JsonToken.PropertyName &&
                        reader.Value.ToString() == fieldName) {
                        await writer.WritePropertyNameAsync(fieldName);
                        await writer.WriteValueAsync(value);
                        currentObjectHasTargetField = true; //to not add target field at end of object
                    } else {
                        switch (reader.TokenType) {
                            case JsonToken.PropertyName:
                                await writer.WritePropertyNameAsync(reader.Value.ToString());
                                break;
                            case JsonToken.Boolean:
                            case JsonToken.Bytes:
                            case JsonToken.Date:
                            case JsonToken.Float:
                            case JsonToken.Integer:
                            case JsonToken.String:
                                await writer.WriteValueAsync(reader.Value);
                                break;
                            case JsonToken.Null:
                                await writer.WriteNullAsync();
                                break;
                            case JsonToken.StartObject:
                                await writer.WriteStartObjectAsync();
                                break;
                            case JsonToken.StartArray:
                                await writer.WriteStartArrayAsync();
                                break;
                            case JsonToken.EndArray:
                                await writer.WriteEndArrayAsync();
                                break;
                            case JsonToken.Comment:
                                await writer.WriteCommentAsync(reader.Value.ToString());
                                break;
                        }
                    }
                }
            }

        }


    }
}
