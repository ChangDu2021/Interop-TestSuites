namespace Microsoft.Protocols.TestSuites.MS_OXORULE
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Protocols.TestSuites.Common;

    /// <summary>
    /// Address book EntryIDs can represent several types of Address Book objects including individual users, distribution lists, containers, and templates.
    /// </summary>
    public class AddressBookEntryID
    {
        /// <summary>
        /// MUST be 0x00000000.
        /// </summary>
        public readonly uint Flags = 0x00000000;

        /// <summary>
        /// The ProviderUID value that MUST be %xDC.A7.40.C8.C0.42.10.1A.B4.B9.08.00.2B.2F.E1.82.
        /// </summary>
        public readonly byte[] ProviderUID = new byte[] { 0xDC, 0xA7, 0x40, 0xC8, 0xC0, 0x42, 0x10, 0x1A, 0xB4, 0xB9, 0x08, 0x00, 0x2B, 0x2F, 0xE1, 0x82 };

        /// <summary>
        /// MUST be set to %x01.00.00.00.
        /// </summary>
        public readonly uint Version = 0x00000001;

        /// <summary>
        /// A 32-bit integer representing the type of the object. 
        /// </summary>
        private ObjectTypes type;

        /// <summary>
        /// The X500 DN of the Address Book object. X500DN is a null-terminated string of 8-bit characters.
        /// </summary>
        private string valueOfX500DN;

        /// <summary>
        /// Initializes a new instance of the AddressBookEntryID class.
        /// </summary>
        public AddressBookEntryID()
        {
        }

        /// <summary>
        /// Initializes a new instance of the AddressBookEntryID class.
        /// </summary>
        /// <param name="valueOfx500DN">The X500 DN of the Address Book object. This must not be a null-terminated string.</param>
        public AddressBookEntryID(string valueOfx500DN)
        {
            this.Type = ObjectTypes.LocalMailUser;
            this.valueOfX500DN = valueOfx500DN;
        }

        /// <summary>
        /// Initializes a new instance of the AddressBookEntryID class.
        /// </summary>
        /// <param name="valueOfx500DN">The X500 DN of the Address Book object. This must not be a null-terminated string.</param>
        /// <param name="types">Type of object.</param>
        public AddressBookEntryID(string valueOfx500DN, ObjectTypes types)
        {
            this.Type = types;
            this.valueOfX500DN = valueOfx500DN;
        }

        /// <summary>
        /// A 32-bit integer representing the type of the object. 
        /// </summary>
        public enum ObjectTypes : uint
        {
            /// <summary>
            /// Local mail user type
            /// </summary>
            LocalMailUser = 0x0000,

            /// <summary>
            /// Distribution list type
            /// </summary>
            DistributionList = 0x0001,

            /// <summary>
            /// Bulletin board or public folder type
            /// </summary>
            BulletinBoardOrPublicFolder = 0x0002,

            /// <summary>
            /// Automated mailbox type
            /// </summary>
            AutomatedMailbox = 0x0003,

            /// <summary>
            /// Organizational mailbox type
            /// </summary>
            OrganizationalMailbox = 0x0004,

            /// <summary>
            /// Private distribution list type
            /// </summary>
            PrivateDistributionList = (ushort)PropertyId.PidTagAutoForwarded,

            /// <summary>
            /// Remote mail user type
            /// </summary>
            RemoteMailUser = 0x0006,

            /// <summary>
            /// Container type
            /// </summary>
            Container = 0x0100,

            /// <summary>
            /// Template type
            /// </summary>
            Template = 0x0101,

            /// <summary>
            /// One-off user type
            /// </summary>
            OneOffUser = 0x0102,

            /// <summary>
            /// Search type
            /// </summary>
            Search = 0x0200
        }

        /// <summary>
        /// Gets or sets the X500 DN of the Address Book object. X500DN is a null-terminated string of 8-bit characters.
        /// </summary>
        public string ValueOfX500DN
        {
            get { return this.valueOfX500DN; }
            set { this.valueOfX500DN = value; }
        }

        /// <summary>
        /// Gets or sets a 32-bit integer representing the type of the object. 
        /// </summary>
        public ObjectTypes Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        /// <summary>
        /// Get size of this class
        /// </summary>
        /// <returns>Size in byte of this class.</returns>
        public int Size()
        {
            return this.Serialize().Length;
        }

        /// <summary>
        /// Get serialized byte array for this struct
        /// </summary>
        /// <returns>Serialized byte array.</returns>
        public byte[] Serialize()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(this.Flags));
            bytes.AddRange(this.ProviderUID);
            bytes.AddRange(BitConverter.GetBytes(this.Version));
            bytes.AddRange(BitConverter.GetBytes((uint)this.Type));
            bytes.AddRange(Encoding.ASCII.GetBytes(this.valueOfX500DN + "\0"));
            return bytes.ToArray();
        }

        /// <summary>
        /// Deserialized byte array to an ActionBlock instance
        /// </summary>
        /// <param name="buffer">Byte array contain data of an ActionBlock instance.</param>
        /// <returns>Bytes count that deserialized in buffer.</returns>
        public uint Deserialize(byte[] buffer)
        {
            BufferReader reader = new BufferReader(buffer);
            uint flags = reader.ReadUInt32();
            if (this.Flags != flags)
            {
                throw new ParseException("Flags MUST be 0x00000000.");
            }

            byte[] providerUID = reader.ReadBytes(16);
            if (!Common.CompareByteArray(this.ProviderUID, providerUID))
            {
                throw new ParseException("ProviderUID MUST be %xDC.A7.40.C8.C0.42.10.1A.B4.B9.08.00.2B.2F.E1.82.");
            }

            uint version = reader.ReadUInt32();
            if (this.Version != version)
            {
                throw new ParseException("Version MUST be 0x00000001.");
            }

            this.Type = (ObjectTypes)reader.ReadUInt32();
            this.valueOfX500DN = reader.ReadASCIIString();

            return reader.Position;
        }
    }
}