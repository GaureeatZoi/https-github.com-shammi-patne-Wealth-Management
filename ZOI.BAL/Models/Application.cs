using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace ZOI.BAL.Models
{
    [Table("tbl_Applications")]
    public class Application : Base.BaseModel
    {
	public long ID { get; set; }
	public string  ApplicationHash  { get; set; }
	public  string ApplicationCode  { get; set; }
	public  string ApplicationName    { get; set; }
	public  string ApplicationLogo  { get; set; }
	public  string ApplicationUrl { get; set; }
	public int ApplicationPlatform     { get; set; }
	public bool  NeedLDAPAuthentication    { get; set; }
	public int ApplicationStatus      { get; set; }
	public string ApplicationDescription { get; set; }
	public string  OpenID_RedirectURL      { get; set; }
	public  string OpenID_ApplicationSecret      { get; set; }
	public  string  SAML_DigitalCertificate     { get; set; }
	public  string SAML_IdentityProvider      { get; set; }
	public  string Custom_PrivateKey      { get; set; }
	public  int ProtocolUsed      { get; set; }
	public bool  MakeDefaultApp     { get; set; }
	public string CertificatePassword      { get; set; }
	public  string SAML_AssertionConsumerServiceUrl  { get; set; }
	public  int TechnologyId     { get; set; }
	public  int BusinessUnitId   { get; set; }
	public  int  ContactpersonId { get; set; }
	public  int CategoryId      { get; set; }
	public string IdentityProvider   { get; set; }
	public string  Custom_PublicKey  { get; set; }
	public string Custom_RedirectURL     { get; set; }


	}
}
