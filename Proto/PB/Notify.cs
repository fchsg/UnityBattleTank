//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: Notify.proto
// Note: requires additional types generated from: PrizeItem.proto
namespace SlgPB
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Notify")]
  public partial class Notify : global::ProtoBuf.IExtensible
  {
    public Notify() {}
    
    private int _notifyId = default(int);
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"notifyId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int notifyId
    {
      get { return _notifyId; }
      set { _notifyId = value; }
    }
    private string _senderName = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"senderName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string senderName
    {
      get { return _senderName; }
      set { _senderName = value; }
    }
    private string _title = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"title", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string title
    {
      get { return _title; }
      set { _title = value; }
    }
    private string _content = "";
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"content", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string content
    {
      get { return _content; }
      set { _content = value; }
    }
    private readonly global::System.Collections.Generic.List<PrizeItem> _prizeItems = new global::System.Collections.Generic.List<PrizeItem>();
    [global::ProtoBuf.ProtoMember(5, Name=@"prizeItems", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<PrizeItem> prizeItems
    {
      get { return _prizeItems; }
    }
  
    private int _sendTime = default(int);
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"sendTime", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int sendTime
    {
      get { return _sendTime; }
      set { _sendTime = value; }
    }
    private int _isRead = default(int);
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"isRead", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int isRead
    {
      get { return _isRead; }
      set { _isRead = value; }
    }
    private int _disposed = default(int);
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"disposed", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int disposed
    {
      get { return _disposed; }
      set { _disposed = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}