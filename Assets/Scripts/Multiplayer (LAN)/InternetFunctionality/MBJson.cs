using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace MBJson
{
    public enum JSONType
    {
        Integer,
        Float,
        Array,
        Aggregate,
        String,
        Boolean,
        Null
    }

    public class JSONObject
    {
        JSONType m_Type = JSONType.Null;
        object m_InternalData = null;

        public JSONType GetJSONType()
        {
            return (m_Type);
        }
        public JSONObject()
        {

        }
        public JSONObject(string StringData)
        {
            m_Type = JSONType.String;
            m_InternalData = StringData;
        }
        public JSONObject(int IntegerData)
        {
            m_Type = JSONType.Integer;
            m_InternalData = IntegerData;
        }
        public JSONObject(List<JSONObject> Contents)
        {
            m_Type = JSONType.Array;
            m_InternalData = Contents;
        }
        public JSONObject(Dictionary<string, JSONObject> Contents)
        {
            m_Type = JSONType.Aggregate;
            m_InternalData = Contents;
        }
        public JSONObject(bool BoolData)
        {
            m_Type = JSONType.Boolean;
            m_InternalData = BoolData;
        }


        public string GetStringData()
        {
            if (m_Type != JSONType.String)
            {
                throw new System.Exception("Object not of string type");
            }
            return ((string)m_InternalData);
        }
        public int GetIntegerData()
        {
            if (m_Type != JSONType.Integer)
            {
                throw new System.Exception("Object not of string type");
            }
            return ((int)m_InternalData);
        }
        public bool GetBooleanData()
        {
            if (m_Type != JSONType.Boolean)
            {
                throw new System.Exception("Object not of boolean type");
            }
            return ((bool)m_InternalData);
        }
        public List<JSONObject> GetArrayData()
        {
            if (m_Type != JSONType.Array)
            {
                throw new System.Exception("Object not of string type");
            }
            return ((List<JSONObject>)m_InternalData);
        }
        public Dictionary<string, JSONObject> GetAggregateData()
        {
            if (m_Type != JSONType.Aggregate)
            {
                throw new System.Exception("Object not of string type");
            }
            return ((Dictionary<string, JSONObject>)m_InternalData);
        }
        public JSONObject this[string MemberName]
        {
            get
            {
                if (m_Type != JSONType.Aggregate)
                {
                    throw new System.Exception("Object not of aggregate type");
                }
                Dictionary<string, JSONObject> AggregateData = (Dictionary<string, JSONObject>)m_InternalData;
                return (AggregateData[MemberName]);
            }
            set
            {
                if (m_Type != JSONType.Aggregate)
                {
                    throw new System.Exception("Object not of aggregate type");
                }
                Dictionary<string, JSONObject> AggregateData = (Dictionary<string, JSONObject>)m_InternalData;
                AggregateData[MemberName] = value;
            }
        }

        public JSONObject Copy()
        {
            JSONObject ReturnValue = null;
            return (ReturnValue);
        }
        static string ParseQuotedString(byte[] Buffer, int Offset, out int OutOffset)
        {
            string ReturnValue = "";
            int ParseOffset = Offset;
            //skipping "
            ParseOffset += 1;
            int NextQuote = FindCharacter(Buffer, ParseOffset, '\"');
            ReturnValue = System.Text.Encoding.UTF8.GetString(Buffer, ParseOffset, NextQuote - ParseOffset);
            //throw new Exception("ParseOffset: " + ParseOffset + " NextQuote: " + NextQuote + " ReturnValue: " + ReturnValue);
            ParseOffset = NextQuote + 1;
            OutOffset = ParseOffset;
            //throw new Exception("Parseoffset: " + ParseOffset);
            return (ReturnValue);
        }


        static void SkipWhiteSpace(byte[] Buffer, int Offset, out int OutOffset)
        {
            int ParseOffset = Offset;
            while (ParseOffset < Buffer.Length)
            {
                if (Buffer[ParseOffset] == ' ' || Buffer[ParseOffset] == '\t' || Buffer[ParseOffset] == '\n')
                {
                    ParseOffset += 1;
                }
                else
                {
                    break;
                }
            }
            OutOffset = ParseOffset;
        }
        static int FindCharacter(byte[] Buffer, int Offset, char CharacterToFind)
        {
            int ReturnValue = Offset;
            while (ReturnValue < Buffer.Length)
            {
                if (Buffer[ReturnValue] == CharacterToFind)
                {
                    break;
                }
                ReturnValue++;
            }

            return (ReturnValue);
        }
        static JSONObject Parse_Boolean(byte[] Buffer, int Offset, out int OutOffset)
        {
            bool ReturnValue = false;
            int ParseOffset = Offset;
            if (Buffer[ParseOffset] == 't')
            {
                if (System.Text.Encoding.UTF8.GetString(Buffer, Offset, 4) != "true")
                {
                    throw new System.Exception("Invalid true data");
                }
                ReturnValue = true;
                ParseOffset += 4;
            }
            else if (Buffer[ParseOffset] == 'f')
            {
                if (System.Text.Encoding.UTF8.GetString(Buffer, Offset, 5) != "false")
                {
                    throw new System.Exception("Invalid true data");
                }
                ParseOffset += 5;
            }
            else
            {
                throw new System.Exception("Invalid begin of boolean type");
            }
            OutOffset = ParseOffset;
            return (new JSONObject(ReturnValue));
        }
        static JSONObject Parse_Integer(byte[] Buffer, int Offset, out int OutOffset)
        {
            int ReturnValue = 0;
            int ParseOffset = Offset;
            int IntegerBegin = ParseOffset;
            int IntEnd = IntegerBegin;
            while (IntEnd < Buffer.Length)
            {
                if (!(Buffer[IntEnd] >= '0' && Buffer[IntEnd] <= '9'))
                {
                    break;
                }
                IntEnd += 1;
            }
            try
            {
                ReturnValue = int.Parse(System.Text.Encoding.UTF8.GetString(Buffer, IntegerBegin, IntEnd - IntegerBegin));
            }
            catch (Exception)
            {
                //throw new Exception("IntEnd " + IntEnd + " IntegerBegin: " + IntegerBegin);
                //throw new Exception("IntEnd " + IntEnd + " IntegerBegin: " + IntegerBegin);
                throw new Exception("Invalid integer when parsing string: " + System.Text.Encoding.UTF8.GetString(Buffer, IntegerBegin, IntEnd - IntegerBegin) + " Whole data: " + System.Text.Encoding.UTF8.GetString(Buffer));
            }

            OutOffset = IntEnd;
            return (new JSONObject(ReturnValue));
        }
        static JSONObject Parse_Null(byte[] Buffer, int Offset, out int OutOffset)
        {
            JSONObject ReturnValue = new JSONObject();
            int ParseOffset = Offset;
            if (System.Text.Encoding.UTF8.GetString(Buffer, Offset, 4) != "null")
            {
                throw new System.Exception("Invalid null data");
            }
            ParseOffset += 4;
            OutOffset = ParseOffset;
            return ReturnValue;
        }
        static JSONObject Parse_String(byte[] Buffer, int Offset, out int OutOffset)
        {
            string ReturnValue = null;
            int ParseOffset = Offset;
            ReturnValue = ParseQuotedString(Buffer, ParseOffset, out ParseOffset);
            //throw new Exception("Parseoffset: " + ParseOffset + " string: " + ReturnValue);
            OutOffset = ParseOffset;
            return (new JSONObject(ReturnValue));
        }
        static JSONObject Parse_Aggregate(byte[] Buffer, int Offset, out int OutOffset)
        {
            Dictionary<string, JSONObject> Contents = new Dictionary<string, JSONObject>();
            int ParseOffset = Offset;
            //skipping {
            ParseOffset += 1;
            bool EndReached = false;
            while (ParseOffset < Buffer.Length)
            {
                SkipWhiteSpace(Buffer, ParseOffset, out ParseOffset);
                if (ParseOffset >= Buffer.Length)
                {
                    throw new System.Exception("Early end of file reached when parsing json object");
                }
                if (Buffer[ParseOffset] == '}')
                {
                    EndReached = true;
                    ParseOffset += 1;
                    break;
                }
                if (Buffer[ParseOffset] != '\"')
                {
                    throw new System.Exception("invalid begin of object member name");
                }
                string MemberName = ParseQuotedString(Buffer, ParseOffset, out ParseOffset);
                SkipWhiteSpace(Buffer, ParseOffset, out ParseOffset);
                if (ParseOffset >= Buffer.Length)
                {
                    throw new System.Exception("Early end of file reached when parsing json object");
                }
                if (Buffer[ParseOffset] != ':')
                {
                    throw new System.Exception("Invalid value delimiter in json object");
                }
                ParseOffset += 1;
                JSONObject Value = ParseJSONObject(Buffer, ParseOffset, out ParseOffset);
                Contents.Add(MemberName, Value);
                SkipWhiteSpace(Buffer, ParseOffset, out ParseOffset);
                if (ParseOffset >= Buffer.Length)
                {
                    throw new System.Exception("Early end of file reached when parsing json object");
                }
                if (Buffer[ParseOffset] == '}')
                {
                    EndReached = true;
                    ParseOffset += 1;
                    break;
                }
                if (Buffer[ParseOffset] != ',')
                {
                    throw new System.Exception("Invalid aggregate delimiter");
                }
                ParseOffset += 1;
            }
            if (!EndReached)
            {
                throw new System.Exception("Early end of file reached when parsing json object");
            }
            OutOffset = ParseOffset;
            return (new JSONObject(Contents));
        }
        static JSONObject Parse_Array(byte[] ByteBuffer, int Offset, out int OutOffset)
        {
            List<JSONObject> Contents = new List<JSONObject>();
            int ParseOffset = Offset;
            //skipping [
            ParseOffset += 1;
            bool EndReached = false;
            SkipWhiteSpace(ByteBuffer, ParseOffset, out ParseOffset);
            while (ParseOffset < ByteBuffer.Length)
            {
                if (ParseOffset >= ByteBuffer.Length)
                {
                    throw new System.Exception("Early end of file reached when parsing json object");
                }
                if (ByteBuffer[ParseOffset] == ']')
                {
                    EndReached = true;
                    ParseOffset += 1;
                    break;
                }
                //int ParseBefore = ParseOffset;
                Contents.Add(ParseJSONObject(ByteBuffer, ParseOffset, out ParseOffset));
                //int ParseAfter = ParseOffset;
                //throw new Exception("Before: " + ParseBefore + " After: " + ParseAfter);
                SkipWhiteSpace(ByteBuffer, ParseOffset, out ParseOffset);
                if (ParseOffset >= ByteBuffer.Length)
                {
                    throw new System.Exception("Early end of file reached when parsing json object");
                }
                if (ByteBuffer[ParseOffset] == ']')
                {
                    EndReached = true;
                    ParseOffset += 1;
                    break;
                }
                if (ByteBuffer[ParseOffset] != ',')
                {
                    throw new System.Exception("Invalid array delimiter: " + ByteBuffer[ParseOffset]);
                }
                ParseOffset += 1;
                SkipWhiteSpace(ByteBuffer, ParseOffset, out ParseOffset);
            }
            if (!EndReached)
            {
                throw new System.Exception("Early end of file reached when parsing json object");
            }
            OutOffset = ParseOffset;
            return (new JSONObject(Contents));
        }

        public static JSONObject ParseJSONObject(byte[] ByteBuffer, int Offset, out int OutOffset)
        {
            JSONObject ReturnValue = null;
            int ParseOffset = Offset;
            SkipWhiteSpace(ByteBuffer, ParseOffset, out ParseOffset);
            if (ParseOffset >= ByteBuffer.Length)
            {
                throw new System.Exception("Unexpected end of file reached when parsing JSON");
            }
            if (ByteBuffer[ParseOffset] == '{')
            {
                ReturnValue = Parse_Aggregate(ByteBuffer, ParseOffset, out ParseOffset);
            }
            else if (ByteBuffer[ParseOffset] == '[')
            {
                ReturnValue = Parse_Array(ByteBuffer, ParseOffset, out ParseOffset);
            }
            else if (ByteBuffer[ParseOffset] == '\"')
            {
                ReturnValue = Parse_String(ByteBuffer, ParseOffset, out ParseOffset);
            }
            else if (ByteBuffer[ParseOffset] == 't' || ByteBuffer[ParseOffset] == 'f')
            {
                ReturnValue = Parse_Boolean(ByteBuffer, ParseOffset, out ParseOffset);
            }
            else if (ByteBuffer[ParseOffset] == 'n')
            {
                ReturnValue = Parse_Null(ByteBuffer, ParseOffset, out ParseOffset);
            }
            else
            {
                ReturnValue = Parse_Integer(ByteBuffer, ParseOffset, out ParseOffset);
                //floats are not supported
            }
            OutOffset = ParseOffset;
            return (ReturnValue);
        }

        public static JSONObject SerializeObject(object ObjectToSerialize)
        {
            JSONObject ReturnValue = null;

            
            Type ObjectType = ObjectToSerialize.GetType();
            if (ObjectToSerialize is string)
            {
                ReturnValue = new JSONObject((string)(object)ObjectToSerialize);
            }
            else if (ObjectToSerialize is int)
            {
                ReturnValue = new JSONObject((int)(object)ObjectToSerialize);
            }
            else if (ObjectToSerialize is bool)
            {
                ReturnValue = new JSONObject((bool)(object)ObjectToSerialize);
            }
            else if (ObjectType.IsEnum)
            {
                ReturnValue = new JSONObject((int)(object)ObjectToSerialize);
            }
            else if (ObjectToSerialize is IDictionary)
            {
                IEnumerable Enumerator = (IEnumerable)ObjectToSerialize;
                Dictionary<string, JSONObject> JsonList = new Dictionary<string, JSONObject>();
                foreach (DictionaryEntry Entry in Enumerator)
                {
                    if (!(Entry.Key is string))
                    {
                        throw new Exception("Invalid key: key must have string type");
                    }
                    JsonList.Add((string)Entry.Key, SerializeObject(Entry.Value));
                }
                ReturnValue = new JSONObject(JsonList);
            }
            else if (ObjectToSerialize is IEnumerable)
            {
                IEnumerable Enumerator = (IEnumerable)ObjectToSerialize;
                List<JSONObject> JsonList = new List<JSONObject>();
                foreach (object ListObject in Enumerator)
                {
                    JsonList.Add(SerializeObject(ListObject));
                }
                ReturnValue = new JSONObject(JsonList);
            }
            else
            {
                FieldInfo[] Fields = ObjectType.GetFields();
                Dictionary<string, JSONObject> JsonDictionary = new Dictionary<string, JSONObject>();
                foreach (FieldInfo Field in Fields)
                {
                    JsonDictionary.Add(Field.Name, SerializeObject(Field.GetValue(ObjectToSerialize)));
                }
                ReturnValue = new JSONObject(JsonDictionary);
            }
            //else if(ObjectToSerialize is Dictionary<string,)
            return (ReturnValue);
        }
        public static T DeserializeObject<T>(JSONObject ObjectToParse)
        {
            //T ReturnValue = new T();
            T ReturnValue = default(T);
            bool Return = false;
            if (typeof(T) == typeof(int))
            {
                Return = true;
                ReturnValue = (T)(object)ObjectToParse.GetIntegerData();
            }
            else if (typeof(T) == typeof(string))
            {
                Return = true;
                ReturnValue = (T)(object)ObjectToParse.GetStringData();
            }
            else if (typeof(T) == typeof(bool))
            {
                Return = true;
                ReturnValue = (T)(object)ObjectToParse.GetBooleanData();
            }
            else if (typeof(T).IsEnum)
            {
                Return = true;
                ReturnValue = (T)(object)ObjectToParse.GetIntegerData();
            }
            if (Return)
            {
                return (ReturnValue);
            }
            ConstructorInfo ConstructorToUse = typeof(T).GetConstructor(Type.EmptyTypes);
            if (ConstructorToUse == null)
            {
                string ErrorString = "Error Deserializing type: no default constructor avaialbe for " + typeof(T).Name;
                throw new Exception(ErrorString);
            }
            ReturnValue = (T)typeof(T).GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
            if (ReturnValue is JSONDeserializeable)
            {
                JSONDeserializeable JsonSerializer = (JSONDeserializeable)ReturnValue;
                ReturnValue = (T)JsonSerializer.Deserialize(ObjectToParse);
            }
            else if (ReturnValue is IDictionary)
            {
                IDictionary DictionaryData = (IDictionary)ReturnValue;
                Type ReturnType = ReturnValue.GetType();
                Dictionary<string, JSONObject> SerializedDictionary = ObjectToParse.GetAggregateData();
                foreach (KeyValuePair<string, JSONObject> SerializedField in SerializedDictionary)
                {
                    object SerializedValue = typeof(JSONObject).GetMethod("DeserializeObject").MakeGenericMethod(ReturnType.GenericTypeArguments[1]).Invoke(null, new object[] { SerializedField.Value });
                    DictionaryData.Add(SerializedField.Key, SerializedValue);
                }
            }
            else if (ReturnValue is IList)
            {
                IList ListToModify = (IList)ReturnValue;
                Type ReturnType = ReturnValue.GetType();
                List<JSONObject> SerializedList = ObjectToParse.GetArrayData();
                foreach (JSONObject ListEntry in SerializedList)
                {
                    object SerializedValue = typeof(JSONObject).GetMethod("DeserializeObject").MakeGenericMethod(ReturnType.GenericTypeArguments[0]).Invoke(null, new object[] { ListEntry });
                    ListToModify.Add(SerializedValue);
                }
            }
            else
            {
                Type ObjectType = ReturnValue.GetType();
                FieldInfo[] Fields = ObjectType.GetFields();
                //Fields[0].FieldType.isen
                Dictionary<string, JSONObject> SerializedObjectData = ObjectToParse.GetAggregateData();
                foreach (FieldInfo Field in Fields)
                {
                    MethodInfo DeserializeMethod = typeof(JSONObject).GetMethod("DeserializeObject");
                    //throw new Exception(Field.Name +" "+ Field.FieldType.ToString());
                    //throw new Exception(DeserializeMethod.ToString());
                    MethodInfo MethodToCall = DeserializeMethod.MakeGenericMethod(Field.FieldType);
                    object SerializedValue = MethodToCall.Invoke(null, new object[] { SerializedObjectData[Field.Name] });
                    Field.SetValue(ReturnValue, SerializedValue);
                }
            }
            return (ReturnValue);
        }




        string ToString_Int()
        {
            return (((int)m_InternalData).ToString());
        }
        string ToString_Boolean()
        {
            bool BooleanValue = (bool)m_InternalData;
            if (BooleanValue)
            {
                return ("true");
            }
            else
            {
                return ("false");
            }
        }
        string ToString_String()
        {
            return ("\"" + (string)m_InternalData + "\"");
        }
        string ToString_Array()
        {
            string ReturnValue = "[";
            List<JSONObject> Values = GetArrayData();
            foreach (JSONObject Value in Values)
            {
                ReturnValue += Value.ToString() + ",";
            }
            if (Values.Count > 0)
            {
                //System.Console.WriteLine("Removing last");
                ReturnValue = ReturnValue.Remove(ReturnValue.Length - 1, 1);

            }
            ReturnValue += "]";
            return (ReturnValue);
        }
        string ToString_Aggregate()
        {
            Dictionary<string, JSONObject> DictionaryData = GetAggregateData();
            string ReturnValue = "{";
            foreach (KeyValuePair<string, JSONObject> Value in DictionaryData)
            {
                ReturnValue += "\"" + Value.Key + "\":" + Value.Value.ToString() + ",";
            }
            if (DictionaryData.Count > 0)
            {
                ReturnValue = ReturnValue.Remove(ReturnValue.Length - 1);
            }
            ReturnValue += "}";
            return (ReturnValue);
        }
        public override string ToString()
        {
            string ReturnValue = "";
            if (m_Type == JSONType.Integer)
            {
                ReturnValue = ToString_Int();
            }
            else if (m_Type == JSONType.Aggregate)
            {
                ReturnValue = ToString_Aggregate();
            }
            else if (m_Type == JSONType.Boolean)
            {
                ReturnValue = ToString_Boolean();
            }
            else if (m_Type == JSONType.Array)
            {
                ReturnValue = ToString_Array();
            }
            else if (m_Type == JSONType.Null)
            {
                ReturnValue = "null";
            }
            else if (m_Type == JSONType.String)
            {
                ReturnValue = ToString_String();
            }
            else
            {
                throw new Exception("Invalid json type when serializing to string: " + m_Type.ToString());
            }
            return (ReturnValue);
        }
    }


    interface JSONDeserializeable
    {
        object Deserialize(JSONObject ObjectToParse);
    }

    interface JSONTypeConverter
    {
        Type GetType(int SerializedType);
    }

    class DynamicJSONDeserializer
    {

        JSONTypeConverter m_Converter;

        public DynamicJSONDeserializer(JSONTypeConverter Converter)
        {
            m_Converter = Converter;
        }
        public object Deserialize(JSONObject ObjectToParse)
        {
            object ReturnValue = null;
            Type ObjectType = m_Converter.GetType(ObjectToParse["Type"].GetIntegerData());
            ConstructorInfo ConstructorToUse = ObjectType.GetConstructor(Type.EmptyTypes);
            if (ConstructorToUse == null)
            {
                throw new Exception("No valid default constructor for type: " + ObjectType.Name);
            }
            ReturnValue = ConstructorToUse.Invoke(new object[] { });
            FieldInfo[] Fields = ObjectType.GetFields();
            //Fields[0].FieldType.isen
            Dictionary<string, JSONObject> SerializedObjectData = ObjectToParse.GetAggregateData();
            foreach (FieldInfo Field in Fields)
            {
                MethodInfo DeserializeMethod = typeof(JSONObject).GetMethod("DeserializeObject");
                MethodInfo MethodToCall = DeserializeMethod.MakeGenericMethod(Field.FieldType);
                object SerializedValue = MethodToCall.Invoke(null, new object[] { SerializedObjectData[Field.Name] });
                Field.SetValue(ReturnValue, SerializedValue);
            }
            return (ReturnValue);
        }
    }
}