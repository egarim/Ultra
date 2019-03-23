using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Ultra.Email
{
    /// <summary>
    /// Implement this interface to be able to send data from your business object as an email
    /// </summary>
    public interface IBoToEmail
    {
        /// <summary>
        /// this method should return the SmtpEmailAccount to be used
        /// </summary>
        /// <returns>the SmtpEmailAccount to send the email</returns>
        SmtpEmailAccount GetEmailAccount();

        /// <summary>
        /// This method should return the subject to be used in the email
        /// </summary>
        /// <returns>The subject</returns>
        string GetSubject();

        /// <summary>
        /// this method should return the body to be used in the email
        /// </summary>
        /// <returns>the body</returns>
        string GetBody();

        /// <summary>
        /// This method should return the carbon copy (CC) recipients to be used in the email, the addresses must be separated by coma
        /// </summary>
        /// <returns>The addresses separated by coma</returns>
        string GetCC();

        /// <summary>
        /// This method should return the blind carbon copy (BCC) recipients to be used in the email, the addresses must be separated by coma
        /// </summary>
        /// <returns>The addresses separated by coma</returns>
        string GetBCC();

        /// <summary>
        /// This method should return the recipients to be used in the email, the addresses must be separated by coma
        /// </summary>
        /// <returns>The addresses separated by coma</returns>
        string GetTo();

        /// <summary>
        /// A list of tuples containing the filename,the content and the content type of the attachments, if you don't want to send attachments just return null
        /// </summary>
        /// <returns>A list of tuples containing the filename,the content and the content type of the attachments</returns>
        List<Tuple<string, MemoryStream, ContentType>> GetAttachments();
    }
}