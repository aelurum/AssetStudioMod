using System.IO;

namespace AssetStudio
{
    public class OffsetStream : Stream
    {
        private readonly Stream _baseStream;
        private readonly long _length;
        private long _offset;

        public override bool CanRead => _baseStream.CanRead;
        public override bool CanSeek => _baseStream.CanSeek;
        public override bool CanWrite => _baseStream.CanWrite;
        public override long Length => _length > 0
            ? _length
            : _baseStream.Length - _offset;

        public override long Position
        {
            get => _baseStream.Position - _offset;
            set => Seek(value, SeekOrigin.Begin);
        }

        public long BasePosition => _baseStream.Position;

        public long Offset
        {
            get => _offset;
            set
            {
                if (value < 0 || value > _baseStream.Length)
                {
                    throw new IOException($"{nameof(Offset)} is out of stream bound");
                }
                _offset = value;
                Seek(0, SeekOrigin.Begin);
            }
        }

        public OffsetStream(FileReader reader)
        {
            _baseStream = reader.BaseStream;
            Offset = reader.Position;
        }

        public OffsetStream(Stream stream, long offset, long length)
        {
            _baseStream = stream;
            _length = length;
            Offset = offset;
        }

        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _baseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (offset > _baseStream.Length)
            {
                throw new IOException("Unable to seek beyond stream bound");
            }

            switch (origin)
            {
                case SeekOrigin.Begin:
                    _baseStream.Seek(offset + _offset, SeekOrigin.Begin);
                    break;
                case SeekOrigin.Current:
                    _baseStream.Seek(offset + Position, SeekOrigin.Begin);
                    break;
                case SeekOrigin.End:
                    _baseStream.Seek(offset + _baseStream.Length, SeekOrigin.Begin);
                    break;
            }
            return Position;
        }

        public override void SetLength(long value)
        {
            throw new System.NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _baseStream.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _baseStream.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
