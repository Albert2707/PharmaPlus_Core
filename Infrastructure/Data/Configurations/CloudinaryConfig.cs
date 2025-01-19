using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Infrastructure.Data.Configurations
{
    public class CloudinaryConfig
    {
        private readonly IConfiguration _config;
        private readonly Cloudinary _cloudinary;

        // Constructor con inyección de dependencias
        public CloudinaryConfig(IConfiguration config)
        {
            _config = config;
            // Configura Cloudinary utilizando la información del archivo appsettings.json
            var cloudinaryAccount = new Account(
                _config["Cloudinary:CloudName"],
                _config["Cloudinary:ApiKey"],
                _config["Cloudinary:ApiSecret"]
            );
            _cloudinary = new Cloudinary(cloudinaryAccount);
        }

        public UploadResult Upload(string fileName, Stream stream)
        {
            // Validación de los parámetros
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("El nombre del archivo no puede estar vacío.", nameof(fileName));

            if (stream == null || !stream.CanRead)
                throw new ArgumentException("El flujo de datos es inválido.", nameof(stream));

            try
            {
                _cloudinary.Api.Secure = true;

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName, stream),
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true
                };

                // Subir la imagen a Cloudinary
                var upload = _cloudinary.Upload(uploadParams);
                Console.WriteLine(upload.JsonObj);

                // Verificar si la URL segura fue generada correctamente
                if (upload.SecureUrl == null)
                {
                    throw new InvalidOperationException("La URL segura de la imagen no fue generada.");
                }

                return new UploadResult
                {
                    SecureUrl = upload.SecureUrl.ToString(),
                    PublicId = upload.PublicId
                };
            }
            catch (Exception ex)
            {
                // Maneja el error y lanza una excepción personalizada si es necesario
                Console.WriteLine($"Error al subir la imagen: {ex.Message}");
                throw;
            }
            finally
            {
                // Cierra el flujo de datos si es necesario
                stream?.Dispose();
            }
        }

        public void Destroy(string publicId)
        {
            try
            {
                var deleteParams = new DeletionParams(publicId)
                {
                    ResourceType = ResourceType.Image,
                    Invalidate = true // Invalida la caché del CDN si es necesario
                };
                _cloudinary.Destroy(deleteParams);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar la imagen: {ex.Message}");
                throw;
            }
        }

        // Clase interna para representar el resultado de la carga
        public class UploadResult
        {
            public string? SecureUrl { get; set; }
            public string? PublicId { get; set; }
        }
    }
}
