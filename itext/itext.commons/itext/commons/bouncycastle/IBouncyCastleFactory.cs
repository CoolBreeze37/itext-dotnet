using System;
using System.Collections.Generic;
using System.IO;
using Java.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.X509;
using iText.Commons.Bouncycastle.Asn1;
using iText.Commons.Bouncycastle.Asn1.Cms;
using iText.Commons.Bouncycastle.Asn1.Esf;
using iText.Commons.Bouncycastle.Asn1.Ess;
using iText.Commons.Bouncycastle.Asn1.Ocsp;
using iText.Commons.Bouncycastle.Asn1.Pkcs;
using iText.Commons.Bouncycastle.Asn1.Util;
using iText.Commons.Bouncycastle.Asn1.X500;
using iText.Commons.Bouncycastle.Asn1.X509;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Cert.Jcajce;
using iText.Commons.Bouncycastle.Cert.Ocsp;
using iText.Commons.Bouncycastle.Cms;
using iText.Commons.Bouncycastle.Cms.Jcajce;
using iText.Commons.Bouncycastle.Operator;
using iText.Commons.Bouncycastle.Operator.Jcajce;
using iText.Commons.Bouncycastle.Tsp;

namespace iText.Commons.Bouncycastle {
    public interface IBouncyCastleFactory {
        IASN1ObjectIdentifier CreateASN1ObjectIdentifier(IASN1Encodable encodable);

        IASN1ObjectIdentifier CreateASN1ObjectIdentifier(String str);

        IASN1ObjectIdentifier CreateASN1ObjectIdentifierInstance(Object @object);

        IASN1InputStream CreateASN1InputStream(Stream stream);

        IASN1InputStream CreateASN1InputStream(byte[] bytes);

        IASN1OctetString CreateASN1OctetString(IASN1Primitive primitive);

        IASN1OctetString CreateASN1OctetString(IASN1Encodable encodable);

        IASN1OctetString CreateASN1OctetString(IASN1TaggedObject taggedObject, bool b);

        IASN1OctetString CreateASN1OctetString(byte[] bytes);

        IASN1Sequence CreateASN1Sequence(Object @object);

        IASN1Sequence CreateASN1Sequence(IASN1Encodable encodable);

        IASN1Sequence CreateASN1Sequence(byte[] array);

        IASN1Sequence CreateASN1SequenceInstance(Object @object);

        IDERSequence CreateDERSequence(IASN1EncodableVector encodableVector);

        IDERSequence CreateDERSequence(IASN1Primitive primitive);

        IASN1TaggedObject CreateASN1TaggedObject(IASN1Encodable encodable);

        IASN1Integer CreateASN1Integer(IASN1Encodable encodable);

        IASN1Integer CreateASN1Integer(int i);

        IASN1Integer CreateASN1Integer(BigInteger i);

        IASN1Set CreateASN1Set(IASN1Encodable encodable);

        IASN1Set CreateASN1Set(Object encodable);

        IASN1Set CreateASN1Set(IASN1TaggedObject taggedObject, bool b);

        IASN1Set CreateNullASN1Set();

        IASN1OutputStream CreateASN1OutputStream(Stream stream);

        IASN1OutputStream CreateASN1OutputStream(Stream outputStream, String asn1Encoding);

        IDEROctetString CreateDEROctetString(byte[] bytes);

        IDEROctetString CreateDEROctetString(IASN1Encodable encodable);

        IASN1EncodableVector CreateASN1EncodableVector();

        IDERNull CreateDERNull();

        IDERTaggedObject CreateDERTaggedObject(int i, IASN1Primitive primitive);

        IDERTaggedObject CreateDERTaggedObject(bool b, int i, IASN1Primitive primitive);

        IDERSet CreateDERSet(IASN1EncodableVector encodableVector);

        IDERSet CreateDERSet(IASN1Primitive primitive);

        IDERSet CreateDERSet(ISignaturePolicyIdentifier identifier);

        IDERSet CreateDERSet(IRecipientInfo recipientInfo);

        IASN1Enumerated CreateASN1Enumerated(int i);

        IASN1Encoding CreateASN1Encoding();

        IAttributeTable CreateAttributeTable(IASN1Set unat);

        IPKCSObjectIdentifiers CreatePKCSObjectIdentifiers();

        IAttribute CreateAttribute(IASN1ObjectIdentifier attrType, IASN1Set attrValues);

        IContentInfo CreateContentInfo(IASN1Sequence sequence);

        IContentInfo CreateContentInfo(IASN1ObjectIdentifier objectIdentifier, IASN1Encodable encodable);

        ITimeStampToken CreateTimeStampToken(IContentInfo contentInfo);

        ISigningCertificate CreateSigningCertificate(IASN1Sequence sequence);

        ISigningCertificateV2 CreateSigningCertificateV2(IASN1Sequence sequence);

        IBasicOCSPResponse CreateBasicOCSPResponse(IASN1Primitive primitive);

        IBasicOCSPResp CreateBasicOCSPResp(IBasicOCSPResponse response);

        IBasicOCSPResp CreateBasicOCSPResp(Object response);

        IOCSPObjectIdentifiers CreateOCSPObjectIdentifiers();

        IAlgorithmIdentifier CreateAlgorithmIdentifier(IASN1ObjectIdentifier algorithm);

        IAlgorithmIdentifier CreateAlgorithmIdentifier(IASN1ObjectIdentifier algorithm, IASN1Encodable encodable);

        Provider CreateProvider();

        String GetProviderName();

        IJceKeyTransEnvelopedRecipient CreateJceKeyTransEnvelopedRecipient(ICipherParameters privateKey);

        IJcaContentVerifierProviderBuilder CreateJcaContentVerifierProviderBuilder();

        IJcaSimpleSignerInfoVerifierBuilder CreateJcaSimpleSignerInfoVerifierBuilder();

        IJcaX509CertificateConverter CreateJcaX509CertificateConverter();

        IJcaDigestCalculatorProviderBuilder CreateJcaDigestCalculatorProviderBuilder();

        ICertificateID CreateCertificateID(IDigestCalculator digestCalculator, IX509CertificateHolder certificateHolder
            , BigInteger bigInteger);

        ICertificateID CreateCertificateID();

        IX509CertificateHolder CreateX509CertificateHolder(byte[] bytes);

        IJcaX509CertificateHolder CreateJcaX509CertificateHolder(X509Certificate certificate);

        IExtension CreateExtension(IASN1ObjectIdentifier objectIdentifier, bool critical, IASN1OctetString octetString
            );

        IExtension CreateExtension();

        IExtensions CreateExtensions(IExtension extension);

        IExtensions CreateNullExtensions();

        IOCSPReqBuilder CreateOCSPReqBuilder();

        ISigPolicyQualifiers CreateSigPolicyQualifiers(params ISigPolicyQualifierInfo[] qualifierInfosBC);

        ISigPolicyQualifierInfo CreateSigPolicyQualifierInfo(IASN1ObjectIdentifier objectIdentifier, IDERIA5String
             @string);

        IASN1String CreateASN1String(IASN1Encodable encodable);

        IASN1Primitive CreateASN1Primitive(IASN1Encodable encodable);

        IOCSPResp CreateOCSPResp(IOCSPResponse ocspResponse);

        IOCSPResp CreateOCSPResp(byte[] bytes);

        IOCSPResp CreateOCSPResp();

        IOCSPResponse CreateOCSPResponse(IOCSPResponseStatus respStatus, IResponseBytes responseBytes);

        IResponseBytes CreateResponseBytes(IASN1ObjectIdentifier asn1ObjectIdentifier, IDEROctetString derOctetString
            );

        IOCSPRespBuilder CreateOCSPRespBuilderInstance();

        IOCSPRespBuilder CreateOCSPRespBuilder();

        IOCSPResponseStatus CreateOCSPResponseStatus(int status);

        IOCSPResponseStatus CreateOCSPResponseStatus();

        ICertificateStatus CreateCertificateStatus();

        IRevokedStatus CreateRevokedStatus(ICertificateStatus certificateStatus);

        IRevokedStatus CreateRevokedStatus(DateTime date, int i);

        IASN1Primitive CreateASN1Primitive(byte[] array);

        IDERIA5String CreateDERIA5String(IASN1TaggedObject taggedObject, bool b);

        IDERIA5String CreateDERIA5String(String str);

        ICRLDistPoint CreateCRLDistPoint(Object @object);

        IDistributionPointName CreateDistributionPointName();

        IGeneralNames CreateGeneralNames(IASN1Encodable encodable);

        IGeneralName CreateGeneralName();

        IOtherHashAlgAndValue CreateOtherHashAlgAndValue(IAlgorithmIdentifier algorithmIdentifier, IASN1OctetString
             octetString);

        ISignaturePolicyId CreateSignaturePolicyId(IASN1ObjectIdentifier objectIdentifier, IOtherHashAlgAndValue algAndValue
            );

        ISignaturePolicyId CreateSignaturePolicyId(IASN1ObjectIdentifier objectIdentifier, IOtherHashAlgAndValue algAndValue
            , ISigPolicyQualifiers policyQualifiers);

        ISignaturePolicyIdentifier CreateSignaturePolicyIdentifier(ISignaturePolicyId policyId);

        IEnvelopedData CreateEnvelopedData(IOriginatorInfo originatorInfo, IASN1Set set, IEncryptedContentInfo encryptedContentInfo
            , IASN1Set set1);

        IRecipientInfo CreateRecipientInfo(IKeyTransRecipientInfo keyTransRecipientInfo);

        IEncryptedContentInfo CreateEncryptedContentInfo(IASN1ObjectIdentifier data, IAlgorithmIdentifier algorithmIdentifier
            , IASN1OctetString octetString);

        ITBSCertificate CreateTBSCertificate(IASN1Encodable encodable);

        IIssuerAndSerialNumber CreateIssuerAndSerialNumber(IX500Name issuer, BigInteger value);

        IRecipientIdentifier CreateRecipientIdentifier(IIssuerAndSerialNumber issuerAndSerialNumber);

        IKeyTransRecipientInfo CreateKeyTransRecipientInfo(IRecipientIdentifier recipientIdentifier, IAlgorithmIdentifier
             algorithmIdentifier, IASN1OctetString octetString);

        IOriginatorInfo CreateNullOriginatorInfo();

        ICMSEnvelopedData CreateCMSEnvelopedData(byte[] valueBytes);

        ITimeStampRequestGenerator CreateTimeStampRequestGenerator();

        ITimeStampResponse CreateTimeStampResponse(byte[] respBytes);

        AbstractOCSPException CreateAbstractOCSPException(Exception e);

        IUnknownStatus CreateUnknownStatus();

        IASN1Dump CreateASN1Dump();

        IASN1BitString CreateASN1BitString(IASN1Encodable encodable);

        IASN1GeneralizedTime CreateASN1GeneralizedTime(IASN1Encodable encodable);

        IASN1UTCTime CreateASN1UTCTime(IASN1Encodable encodable);

        IJcaCertStore CreateJcaCertStore(IList<X509Certificate> certificates);

        ITimeStampResponseGenerator CreateTimeStampResponseGenerator(ITimeStampTokenGenerator tokenGenerator, ICollection
            <String> algorithms);

        ITimeStampRequest CreateTimeStampRequest(byte[] bytes);

        IJcaContentSignerBuilder CreateJcaContentSignerBuilder(String algorithm);

        IJcaSignerInfoGeneratorBuilder CreateJcaSignerInfoGeneratorBuilder(IDigestCalculatorProvider digestCalcProviderProvider
            );

        ITimeStampTokenGenerator CreateTimeStampTokenGenerator(ISignerInfoGenerator siGen, IDigestCalculator dgCalc
            , IASN1ObjectIdentifier policy);

        IX500Name CreateX500Name(X509Certificate certificate);

        IX500Name CreateX500Name(String s);

        IRespID CreateRespID(IX500Name x500Name);

        IBasicOCSPRespBuilder CreateBasicOCSPRespBuilder(IRespID respID);

        IOCSPReq CreateOCSPReq(byte[] requestBytes);

        IX509v2CRLBuilder CreateX509v2CRLBuilder(IX500Name x500Name, DateTime thisUpdate);

        IJcaX509v3CertificateBuilder CreateJcaX509v3CertificateBuilder(X509Certificate signingCert, BigInteger certSerialNumber
            , DateTime startDate, DateTime endDate, IX500Name subjectDnName, AsymmetricKeyParameter publicKey);

        IBasicConstraints CreateBasicConstraints(bool b);

        IKeyUsage CreateKeyUsage();

        IKeyUsage CreateKeyUsage(int i);

        IKeyPurposeId CreateKeyPurposeId();

        IExtendedKeyUsage CreateExtendedKeyUsage(IKeyPurposeId purposeId);

        IX509ExtensionUtils CreateX509ExtensionUtils(IDigestCalculator digestCalculator);

        ISubjectPublicKeyInfo CreateSubjectPublicKeyInfo(Object obj);

        ICRLReason CreateCRLReason();
    }
}
