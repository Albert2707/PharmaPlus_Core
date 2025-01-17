using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public class CloudinaryConfig
    {
        private readonly Cloudinary cloudinary = new("cloudinary://115585149439482:4Hs-tSJX_I6WQKE2S5AhDVGvM4U@dxpkhjxla");
        public UploadResult Upload(string fileName, Stream stream)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("El nombre del archivo no puede estar vacío.", nameof(fileName));

            if (stream == null || !stream.CanRead)
                throw new ArgumentException("El flujo de datos es inválido.", nameof(stream));

            try
            {
                cloudinary.Api.Secure = true;

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName, stream),
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true
                };

                var upload = cloudinary.Upload(uploadParams);
                Console.WriteLine(upload.JsonObj);
                if (upload.SecureUrl == null)
                {
                    throw new InvalidOperationException("La URL segura de la imagen no fue generada.");
                }

                return new UploadResult { SecureUrl=upload.SecureUrl.ToString(),PublicId=upload.PublicId };
            }
            catch (Exception ex)
            {
                // Maneja el error y lanza una excepción personalizada si es necesario
                Console.WriteLine($"Error al subir la imagen: {ex.Message}");
                throw;
            }
            finally
            {
                stream?.Dispose(); // Cierra el flujo si es necesario
            }
        }
        public void Destroy(string pId)
        {
            try
            {
                var deleteImage = new DeletionParams(pId)
                {
                    ResourceType = ResourceType.Image,
                    Invalidate = true // Invalida la caché del CDN si es necesario
                };
                cloudinary.Destroy(deleteImage);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public class UploadResult
        {
            public string SecureUrl { get; set; }
            public string PublicId { get; set; }
        }
    }
}
