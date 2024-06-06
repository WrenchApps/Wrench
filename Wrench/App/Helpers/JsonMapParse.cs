using App.Settings.ContractMap;
using System.Text.Json.Nodes;

namespace App.Helpers
{
    public class JsonMapParse
    {
        private JsonObject _jsonObject;
        private ContractMapSetting _contractMapSetting;

        public JsonMapParse(JsonObject jsonObject, ContractMapSetting contractMapSetting)
        {
            _jsonObject = jsonObject;
            _contractMapSetting = contractMapSetting;
        }

        public JsonObject MapParse()
        {
            JsonParsePropertiesRun();
            JsonArrayParsePropertiesRun();

            JsonRemovePropertiesRun();
            return _jsonObject;
        }

        // see how can do better performance
        private void JsonParsePropertiesRun()
        {
            foreach (var mapFromTo in _contractMapSetting.MapFromTo)
            {
                var Properties = mapFromTo.Split(":");
                var (keyFrom, keyTo) = (Properties[0], Properties[1]);

                var keyFromSplited = keyFrom.Split(".");
                var keyToSplited = keyTo.Split(".");
                JsonParsePropertiesRun(keyFromSplited, keyToSplited);
            }
        }

        private void JsonParsePropertiesRun(string[] mapFromSplited, string[] mapToSplited)
        {
            JsonNode mapFromJsonNode = null;
            JsonNode mapToJsonNode = null;
            JsonObject jsonObjectCurrent = _jsonObject;

            foreach (var Property in mapFromSplited)
            {
                if (jsonObjectCurrent.TryGetPropertyValue(Property, out mapFromJsonNode))
                {
                    if (mapFromJsonNode.GetType() == typeof(JsonObject))
                        jsonObjectCurrent = mapFromJsonNode as JsonObject;
                    else
                        jsonObjectCurrent.Remove(Property);
                }
            }

            jsonObjectCurrent = _jsonObject;
            if (mapFromJsonNode != null)
            {
                var totalSplited = mapToSplited.Length;
                var index = 0;
                foreach (var Property in mapToSplited)
                {
                    index++;

                    if (index == totalSplited)
                        jsonObjectCurrent.Add(Property, mapFromJsonNode);
                    else
                    {
                        if (jsonObjectCurrent.TryGetPropertyValue(Property, out mapToJsonNode))
                        {
                            if (mapToJsonNode.GetType() == typeof(JsonObject))
                                jsonObjectCurrent = mapToJsonNode as JsonObject;
                        }
                        else
                        {
                            var nextJsonObjectCurrent = new JsonObject();
                            jsonObjectCurrent.TryAdd(Property, nextJsonObjectCurrent);
                            jsonObjectCurrent = nextJsonObjectCurrent;
                        }
                    }
                }
            }
        }

        private void JsonRemovePropertiesRun()
        {
            if (_contractMapSetting.Remove == null)
                return;

            foreach (var removeProperty in _contractMapSetting.Remove)
            {
                var removePropertiesplited = removeProperty.Split(".");
                JsonNode jsonNode = null;
                JsonObject jsonObjectCurrent = _jsonObject;

                var totalSplited = removePropertiesplited.Length;
                var index = 0;

                foreach (var Property in removePropertiesplited)
                {
                    index++;
                    if (index == totalSplited)
                        jsonObjectCurrent.Remove(Property);
                    else
                    {
                        if (jsonObjectCurrent.TryGetPropertyValue(Property, out jsonNode))
                            if (jsonNode.GetType() == typeof(JsonObject))
                                jsonObjectCurrent = jsonNode as JsonObject;
                    }
                }
            }
        }


        private void JsonArrayParsePropertiesRun()
        {
            if (_contractMapSetting.MapArray?.Count > 0)
            {
                foreach (var mapArray in _contractMapSetting.MapArray)
                {
                    var Properties = mapArray.ArrayMapFromTo.Split(":");
                    var (keyFrom, keyTo) = (Properties[0], Properties[1]);
                    string[] mapFromSplited = keyFrom.Split(".");
                    string[] mapToSplited = keyTo.Split(".");
                    var (mapFrom, mapTo) = JsonArrayParsePropertiesRun(mapFromSplited, mapToSplited);
                    MapJsonArrayFromTo(mapArray, mapFrom, mapTo);
                }
            }
        }

        private (JsonArray mapFrom, JsonArray mapTo) JsonArrayParsePropertiesRun(string[] mapFromSplited, string[] mapToSplited)
        {
            JsonObject jsonObjectCurrent = _jsonObject;
            JsonArray jsonArrayMapFrom = null;

            foreach (var Property in mapFromSplited)
            {
                if (jsonObjectCurrent.TryGetPropertyValue(Property, out var mapFromJsonNode))
                {
                    if (jsonObjectCurrent.TryGetPropertyValue(Property, out var mapToJsonNode))
                    {
                        if (mapToJsonNode == null)
                            continue;

                        if (mapToJsonNode.GetType() == typeof(JsonObject))
                            jsonObjectCurrent = mapToJsonNode as JsonObject;
                        else if (mapFromJsonNode.GetType() == typeof(JsonArray))
                        {
                            jsonArrayMapFrom = mapFromJsonNode as JsonArray;
                            break;
                        }
                    }
                }
            }

            if (jsonArrayMapFrom == null)
                return (null, null);

            JsonArray jsonArrayMapTo = null;
            jsonObjectCurrent = _jsonObject;
            var totalSplited = mapToSplited.Length;
            var index = 0;

            foreach (var Property in mapToSplited)
            {
                index++;
                if (jsonObjectCurrent.TryGetPropertyValue(Property, out var mapToJsonNode))
                {
                    if (mapToJsonNode.GetType() == typeof(JsonObject))
                        jsonObjectCurrent = mapToJsonNode as JsonObject;
                    else if (mapToJsonNode.GetType() == typeof(JsonArray))
                        jsonArrayMapTo = mapToJsonNode as JsonArray;

                }
                else
                {
                    if (index == totalSplited)
                    {
                        jsonArrayMapTo = new JsonArray();
                        jsonObjectCurrent.Add(Property, jsonArrayMapTo);
                    }
                    else
                    {
                        var jsonObject = new JsonObject();
                        jsonObjectCurrent.Add(Property, jsonObject);
                        jsonObjectCurrent = jsonObject;
                    }
                }
            }


            return (jsonArrayMapFrom, jsonArrayMapTo);
        }

        private void MapJsonArrayFromTo(ContractMapArray contractMapArray, JsonArray mapFrom, JsonArray mapTo)
        {
            if (mapFrom == null || mapTo == null)
                return;

            if (contractMapArray.MapFromTo?.Count > 0)
            {
                foreach (var jsonNode in mapFrom.ToArray())
                {
                    foreach (var mapFromTo in contractMapArray.MapFromTo)
                    {
                        var jsonObject = jsonNode as JsonObject;
                        if (jsonObject != null)
                        {
                            var Properties = mapFromTo.Split(":");
                            var (keyFrom, keyTo) = (Properties[0], Properties[1]);

                            if (jsonObject.TryGetPropertyValue(keyFrom, out var mapToJsonNode))
                            {
                                jsonObject.Remove(keyFrom);
                                jsonObject.Add(keyTo, mapToJsonNode);
                            }
                        }
                    }

                    mapFrom.Remove(jsonNode);
                    mapTo.Add(jsonNode);
                }
            }
            else
            {
                foreach (var jsonNode in mapFrom.ToArray())
                {
                    mapFrom.Remove(jsonNode);
                    mapTo.Add(jsonNode);
                }
            }
        }
    }
}
