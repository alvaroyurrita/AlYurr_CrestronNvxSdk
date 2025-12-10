using System.Buffers;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AlYurr_CrestronNvxSdk.HelperFunctions;
//https://github.com/dotnet/runtime/issues/31433
public static class JsonMerge
{
    public static string Merge(string originalJson, string newContent)
    {
        var jDoc1 = JsonNode.Parse(originalJson);
        var jDoc2 = JsonNode.Parse(newContent);
        if (jDoc1 == null || jDoc2 == null)
            return originalJson;
        var resultingNode = Merge(jDoc1, jDoc2);
        var newState = resultingNode.ToString();
        return newState;
    }
    /// <summary>
    /// Merges the specified Json Node into the base JsonNode for which this method is called.
    /// It is null safe and can be easily used with null-check & null coalesce operators for fluent calls.
    /// NOTE: JsonNodes are context aware and track their parent relationships therefore to merge the values both JsonNode objects
    ///         specified are mutated. The Base is mutated with new data while the source is mutated to remove reverences to all
    ///         fields so that they can be added to the base.
    ///
    /// Source taken directly from the open-source Gist here:
    /// https://gist.github.com/cajuncoding/bf78bdcf790782090d231590cbc2438f
    ///
    /// </summary>
    /// <param name="jsonBase"></param>
    /// <param name="jsonMerge"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private static JsonNode Merge(this JsonNode jsonBase, JsonNode jsonMerge)
    {
        if (jsonBase == null || jsonMerge == null)
            return jsonBase;
        switch (jsonBase)
        {
            case JsonObject jsonBaseObj when jsonMerge is JsonObject jsonMergeObj:
            {
                //NOTE: We must materialize the set (e.g. to an Array), and then clear the merge array so the node can then be 
                //      re-assigned to the target/base Json; clearing the Object seems to be the most efficient approach...
                var mergeNodesArray = jsonMergeObj.ToArray();
                jsonMergeObj.Clear();
                foreach (var prop in mergeNodesArray)
                {
                    if (jsonBaseObj[prop.Key] is JsonObject jsonBaseChildObj &&
                        prop.Value is JsonObject jsonMergeChildObj)
                        jsonBaseObj[prop.Key] = jsonBaseChildObj.Merge(jsonMergeChildObj);
                    else if (jsonBaseObj[prop.Key] is JsonArray jsonBase2ChildObj &&
                             prop.Value is JsonArray jsonMerge2ChildObj)
                        jsonBaseObj[prop.Key] = jsonBase2ChildObj.Merge(jsonMerge2ChildObj);
                    else
                        jsonBaseObj[prop.Key] = prop.Value;
                }
                break;
            }
            case JsonArray jsonBaseArray when jsonMerge is JsonArray jsonMergeArray:
            {
                //NOTE: We must materialize the set (e.g. to an Array), and then clear the merge array,
                //      so they can then be re-assigned to the target/base Json...
                var mergeNodesArray = jsonMergeArray.ToArray();
                jsonMergeArray.Clear();
                //Instead of adding an array to the collection, each array is merged by array location.
                //NVX seem to return array collections in order.
                //An exhaustive testing of every NVX node has not been done.  It is possible that certain
                //nodes might want to be merged differently. In that case, a global merge strategy (used here) will not work
                //and every node will need to be treated individually.
                //This loop will need to include the following conditional line
                //foreach(var mergeNode in mergeNodesArray) jsonBaseArray.Add(mergeNode);
                //based on some condition that will need to be added to the method
                //(merge or add for example)
                for (var i = 0; i < mergeNodesArray.Length; i++)
                {
                    var jsonBaseChild = jsonBaseArray[i];
                    var jsonMergeChild = mergeNodesArray[i];
                    if (jsonBaseChild == null || jsonMergeChild == null)
                        continue;
                    jsonBaseChild.Merge(jsonMergeChild);
                }
                break;
            }
            default:
                throw new ArgumentException(
                    $"The JsonNode type [{jsonBase.GetType().Name}] is incompatible for merging with the target/base " +
                    $"type [{jsonMerge.GetType().Name}]; merge requires the types to be the same.");
        }
        return jsonBase;
    }
    /// <summary>
    /// Merges the specified Dictionary of values into the base JsonNode for which this method is called.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="jsonBase"></param>
    /// <param name="dictionary"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static JsonNode MergeDictionary<TKey, TValue>(this JsonNode jsonBase, IDictionary<TKey, TValue> dictionary,
        JsonSerializerOptions options = null)
        => jsonBase.Merge(JsonSerializer.SerializeToNode(dictionary, options));
}