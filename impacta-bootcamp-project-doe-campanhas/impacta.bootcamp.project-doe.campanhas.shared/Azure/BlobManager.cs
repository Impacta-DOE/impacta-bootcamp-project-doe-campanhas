using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
namespace impacta.bootcamp.project_doe.campanhas.shared.Azure
{
    public class BlobManager : IDisposable
    {
        public string ConnectionStringStorage { get; set; }
        public string ContainerName { get; set; }
        public string BlobName { get; set; }
        public string LocalFilePath { get; set; }
        public Stream FileStream { get; set; }

        private CloudStorageAccount _account { get; set; }
        private BlobContainerClient _container { get; set; }
        private BlobClient _blob { get; set; }
        private bool _ispublic { get; set; }
        public static ArquivoBlob BuilderArquivoBlob(string base64Data, fileType tipoArquivo)
        {
            ArquivoBlob file = new ArquivoBlob();

            try
            {
                file.base64Data = base64Data;
                file.name = Guid.NewGuid().ToString();
                file.name = file.name.Replace("-", string.Empty);
                file.type = tipoArquivo;

                switch (tipoArquivo)
                {
                    case fileType.VIDEO:
                        file.name += ".webm";
                        file.contentType = "video/webm";
                        break;

                    case fileType.IMAGE:
                        file.name += ".png";
                        file.contentType = "image/png";
                        break;

                    case fileType.PDF:
                        file.name += ".pdf";
                        file.contentType = "application/pdf";
                        break;

                    case fileType.CSV:
                        file.name += ".csv";
                        file.contentType = "application/csv";
                        break;

                    case fileType.JSON:
                        file.name += ".json";
                        file.contentType = "application/json";
                        break;

                    default:
                        //do nothing
                        break;
                }

            }
            catch (Exception e)
            {
                throw e;
            }


            return file;
        }

        public BlobManager(string connectionStringStorage, string containerName, string blobName, bool ispublic = false)
        {
            ConnectionStringStorage = connectionStringStorage;
            ContainerName = containerName;
            BlobName = blobName;
            _ispublic = ispublic;
            _container = new BlobContainerClient(ConnectionStringStorage, ContainerName);
            _blob = _container.GetBlobClient(BlobName);
            _account = CloudStorageAccount.Parse(ConnectionStringStorage);

        }

        public void WriteStream(Stream stream)
        {
            FileStream = stream;
        }

        public void gravaArquivoBlobStorage(ArquivoBlob arquivo)
        {

            if (string.IsNullOrWhiteSpace(arquivo.name))
            {
                //Shared.Utils.LogManager.EscreverLog(Models.TipoLog.ERROR, MethodBase.GetCurrentMethod().Name + " - Nome do arquivo nao informado! Impossivel gravar @blob.");

            }
            else if (string.IsNullOrWhiteSpace(arquivo.base64Data))
            {
                //Shared.Utils.LogManager.EscreverLog(Models.TipoLog.ERROR, MethodBase.GetCurrentMethod().Name + " - Base64 nao informado! Impossivel gravar @blob o arquivo:" + arquivo.name);

            }
            else if (string.IsNullOrWhiteSpace(ContainerName))
            {

                //Shared.Utils.LogManager.EscreverLog(Models.TipoLog.ERROR, " - Container vazio! Impossivel gravar o arquivo: " + arquivo.name);
            }
            else
            {

                _container.CreateIfNotExists();
                if (_ispublic)
                {
                    _container.SetAccessPolicy(PublicAccessType.Blob);
                }




                var blobHttpHeader = new BlobHttpHeaders();
                if (!string.IsNullOrWhiteSpace(arquivo.contentType))
                {

                    blobHttpHeader.ContentType = arquivo.contentType;

                }

                var bytes = Convert.FromBase64String(arquivo.base64Data); // sem prefixos (ex data:image/jpeg;base64), apenas o base64.
                using (var stream = new MemoryStream(bytes))
                {

                    _blob.Upload(stream, httpHeaders: blobHttpHeader);
                }
                //Shared.Utils.LogManager.EscreverLog(Models.TipoLog.INFO, " - Arquivo gravado com sucesso @BlobAzure:  " + arquivo.name);
                //;


                #region teste (retrieve data)
                //getFileDataFromBlob(cadastroId + "/" + fileName);
                #endregion


            }

        }

        public string UploadFileBase64Blob(ArquivoBlob arquivo)
        {
            _container.CreateIfNotExists();
            if (_ispublic)
            {
                _container.SetAccessPolicy(PublicAccessType.Blob);
            }

            var blobHttpHeader = new BlobHttpHeaders();
            if (!string.IsNullOrWhiteSpace(arquivo.contentType))
            {

                blobHttpHeader.ContentType = arquivo.contentType;

            }

            var bytes = Convert.FromBase64String(arquivo.base64Data); // sem prefixos (ex data:image/jpeg;base64), apenas o base64.
            using (var stream = new MemoryStream(bytes))
            {

                _blob.Upload(stream, httpHeaders: blobHttpHeader);
            }
            //Shared.Utils.LogManager.EscreverLog(Models.TipoLog.INFO, " - Arquivo gravado com sucesso @BlobAzure:  " + arquivo.name);


            //retornando a URL do Arquivo
            return _blob.Name;


        }

        public async Task<string> UploadFileLargeBlob(FileInfo file)
        {
            _container.CreateIfNotExists();
            CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(ConnectionStringStorage);
            CloudBlobClient BlobClient = StorageAccount.CreateCloudBlobClient();
            CloudBlobContainer Container = BlobClient.GetContainerReference(_container.Name);
            if (_ispublic)
            {
                _container.SetAccessPolicy(PublicAccessType.Blob);
            }
            await Container.CreateIfNotExistsAsync();
            CloudBlockBlob blob = Container.GetBlockBlobReference(file.FullName);
            HashSet<string> blocklist = new HashSet<string>();
            const int pageSizeInBytes = 10485760;
            long prevLastByte = 0;
            long bytesRemain = file.Length;

            byte[] bytess;

            var blobHttpHeader = new BlobHttpHeaders();

            using (var stream = new MemoryStream())
            {
                var fileStream = file.OpenRead();
                await fileStream.CopyToAsync(stream);
                bytess = stream.ToArray();

            }
            //Shared.Utils.LogManager.EscreverLog(Models.TipoLog.INFO, " - Arquivo gravado com sucesso @BlobAzure:  " + arquivo.name);
            // Upload each piece
            do
            {
                long bytesToCopy = Math.Min(bytesRemain, pageSizeInBytes);
                byte[] bytesToSend = new byte[bytesToCopy];

                Array.Copy(bytess, prevLastByte, bytesToSend, 0, bytesToCopy);
                prevLastByte += bytesToCopy;
                bytesRemain -= bytesToCopy;

                //create blockId
                string blockId = Guid.NewGuid().ToString();
                string base64BlockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(blockId));

                await blob.PutBlockAsync(
                    base64BlockId,
                    new MemoryStream(bytesToSend, true),
                    null
                    );

                blocklist.Add(base64BlockId);

            } while (bytesRemain > 0);

            //post blocklist
            await blob.PutBlockListAsync(blocklist);

            //retornando a URL do Arquivo
            return blob.Name;


        }

        public async Task<string> UploadFileLargeBlob(FileInfo file, string pathBlob)
        {
            _container.CreateIfNotExists();
            CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(ConnectionStringStorage);
            CloudBlobClient BlobClient = StorageAccount.CreateCloudBlobClient();
            CloudBlobContainer Container = BlobClient.GetContainerReference(_container.Name);
            if (_ispublic)
            {
                _container.SetAccessPolicy(PublicAccessType.Blob);
            }
            await Container.CreateIfNotExistsAsync();
            CloudBlockBlob blob = Container.GetBlockBlobReference(pathBlob);
            HashSet<string> blocklist = new HashSet<string>();
            const int pageSizeInBytes = 10485760;
            long prevLastByte = 0;
            long bytesRemain = file.Length;

            byte[] bytess;

            var blobHttpHeader = new BlobHttpHeaders();

            using (var stream = new MemoryStream())
            {
                var fileStream = file.OpenRead();
                await fileStream.CopyToAsync(stream);
                bytess = stream.ToArray();

            }
            //Shared.Utils.LogManager.EscreverLog(Models.TipoLog.INFO, " - Arquivo gravado com sucesso @BlobAzure:  " + arquivo.name);
            // Upload each piece
            do
            {
                long bytesToCopy = Math.Min(bytesRemain, pageSizeInBytes);
                byte[] bytesToSend = new byte[bytesToCopy];

                Array.Copy(bytess, prevLastByte, bytesToSend, 0, bytesToCopy);
                prevLastByte += bytesToCopy;
                bytesRemain -= bytesToCopy;

                //create blockId
                string blockId = Guid.NewGuid().ToString();
                string base64BlockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(blockId));

                await blob.PutBlockAsync(
                    base64BlockId,
                    new MemoryStream(bytesToSend, true),
                    null
                    );

                blocklist.Add(base64BlockId);

            } while (bytesRemain > 0);

            //post blocklist
            await blob.PutBlockListAsync(blocklist);

            //retornando a URL do Arquivo
            return blob.Name;


        }
        //Método que salvo um arquivo no Azure Blob e retorna o link para baixar o arquivo
        public async Task<string> GetLinkUploadFile(FileInfo file, string pathBlob)
        {
            _container.CreateIfNotExists();
            var policyName = "testpolicy";
            CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(ConnectionStringStorage);
            CloudBlobClient BlobClient = StorageAccount.CreateCloudBlobClient();
            CloudBlobContainer Container = BlobClient.GetContainerReference(_container.Name);
            if (_ispublic)
            {
                _container.SetAccessPolicy(PublicAccessType.Blob);
            }
            await Container.CreateIfNotExistsAsync();
            CloudBlockBlob blob = Container.GetBlockBlobReference(pathBlob);
            HashSet<string> blocklist = new HashSet<string>();
            const int pageSizeInBytes = 10485760;
            long prevLastByte = 0;
            long bytesRemain = file.Length;

            byte[] bytess;

            var blobHttpHeader = new BlobHttpHeaders();

            using (var stream = new MemoryStream())
            {
                var fileStream = file.OpenRead();
                await fileStream.CopyToAsync(stream);
                bytess = stream.ToArray();

            }
            //Shared.Utils.LogManager.EscreverLog(Models.TipoLog.INFO, " - Arquivo gravado com sucesso @BlobAzure:  " + arquivo.name);
            // Upload each piece
            do
            {
                long bytesToCopy = Math.Min(bytesRemain, pageSizeInBytes);
                byte[] bytesToSend = new byte[bytesToCopy];

                Array.Copy(bytess, prevLastByte, bytesToSend, 0, bytesToCopy);
                prevLastByte += bytesToCopy;
                bytesRemain -= bytesToCopy;

                //create blockId
                string blockId = Guid.NewGuid().ToString();
                string base64BlockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(blockId));

                await blob.PutBlockAsync(
                    base64BlockId,
                    new MemoryStream(bytesToSend, true),
                    null
                    );

                blocklist.Add(base64BlockId);

            } while (bytesRemain > 0);

            //post blocklist
            await blob.PutBlockListAsync(blocklist);

            var storedPolicy = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddYears(1),
                Permissions = SharedAccessBlobPermissions.Read |
                  SharedAccessBlobPermissions.Write |
                  SharedAccessBlobPermissions.List
            };

            var permissions = Container.GetPermissions();

            // optionally clear out any existing policies on this container
            permissions.SharedAccessPolicies.Clear();
            // add in the new one
            permissions.SharedAccessPolicies.Add(policyName, storedPolicy);
            // save back to the container
            Container.SetPermissions(permissions);

            // Now we are ready to create a shared access signature based on the stored access policy
            var containerSignature = Container.GetSharedAccessSignature(null, policyName);
            //retornando a URL do Arquivo
            return blob.Uri + containerSignature;


        }
        public string UploadFileBlobStorage(string caminhoArquivo)
        {

            _blob.Upload(caminhoArquivo);

            //retornando a URL do Arquivo
            return _blob.Name;

        }
        public string DownloadBlobStorageByName(string filename)
        {

            CloudBlobClient _blobClient = _account.CreateCloudBlobClient();
            CloudBlobContainer container = _blobClient.GetContainerReference(BlobName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
            string retornoBlob = string.Empty;
            using (var memoryStream = new MemoryStream())
            {
                blockBlob.DownloadToStream(memoryStream);
                retornoBlob = System.Text.Encoding.Default.GetString(memoryStream.ToArray());
            }

            return retornoBlob;
        }

        public async Task<bool> DownloadLargeBlobLocal(string caminho, string filename)
        {

            CloudBlobClient _blobClient = _account.CreateCloudBlobClient();
            CloudBlobContainer container = _blobClient.GetContainerReference(BlobName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);

            await blockBlob.DownloadToFileAsync(caminho, FileMode.Create);

            if (File.Exists(filename))
                return true;
            else
                return false;
        }
        public async Task<bool> ExistsFileByName(string filename)
        {

            CloudBlobClient _blobClient = _account.CreateCloudBlobClient();
            CloudBlobContainer container = _blobClient.GetContainerReference(ContainerName);

            return await container.GetBlockBlobReference(filename).ExistsAsync();
        }

        public BlobContentInfo UploadStream()
        {
            return _blob.Upload(FileStream);
        }

        public async Task<BlobContentInfo> UploadStreamAsync()
        {
            return await _blob.UploadAsync(FileStream);
        }

        private BlobContainerInfo CreateContainer()
        {
            return _container.Create();
        }

        private async Task<BlobContainerInfo> CreateContainerAsync()
        {
            return await _container.CreateIfNotExistsAsync();
        }

        public void Dispose()
        {
            _container = null;
            _blob = null;
        }
    }
    public class ArquivoBlob
    {
        public string name { get; set; }
        public string contentType { get; set; }
        public string base64Data { get; set; }
        public fileType type { get; set; }
    }

    public enum fileType
    {
        VIDEO = 1,
        IMAGE = 2,
        PDF = 3,
        INDEFINIDO = 4,
        CSV = 5,
        JSON = 6
    }

}

