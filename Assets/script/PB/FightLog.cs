//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: FightLog.proto
namespace SlgPB
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"FightLog")]
  public partial class FightLog : global::ProtoBuf.IExtensible
  {
    public FightLog() {}
    
    private string _oppUserName = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"oppUserName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string oppUserName
    {
      get { return _oppUserName; }
      set { _oppUserName = value; }
    }
    private int _timeToNow = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"timeToNow", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int timeToNow
    {
      get { return _timeToNow; }
      set { _timeToNow = value; }
    }
    private int _fightResult = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"fightResult", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int fightResult
    {
      get { return _fightResult; }
      set { _fightResult = value; }
    }
    private int _direct = default(int);
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"direct", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int direct
    {
      get { return _direct; }
      set { _direct = value; }
    }
    private int _rankChange = default(int);
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"rankChange", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int rankChange
    {
      get { return _rankChange; }
      set { _rankChange = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}