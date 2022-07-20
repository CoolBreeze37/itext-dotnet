using Org.BouncyCastle.Asn1.X509;
using iText.Bouncycastlefips.Asn1;
using iText.Commons.Bouncycastle.Asn1.X509;

namespace iText.Bouncycastlefips.Asn1.X509 {
    public class KeyPurposeIdBCFips : ASN1EncodableBCFips, IKeyPurposeId {
        private static readonly iText.Bouncycastlefips.Asn1.X509.KeyPurposeIdBCFips INSTANCE = new iText.Bouncycastlefips.Asn1.X509.KeyPurposeIdBCFips
            (null);

        private static readonly iText.Bouncycastlefips.Asn1.X509.KeyPurposeIdBCFips ID_KP_OCSP_SIGNING = new iText.Bouncycastlefips.Asn1.X509.KeyPurposeIdBCFips
            (KeyPurposeId.id_kp_OCSPSigning);

        public KeyPurposeIdBCFips(KeyPurposeId KeyPurposeId)
            : base(KeyPurposeId) {
        }

        public static iText.Bouncycastlefips.Asn1.X509.KeyPurposeIdBCFips GetInstance() {
            return INSTANCE;
        }

        public virtual KeyPurposeId GetKeyPurposeId() {
            return (KeyPurposeId)GetEncodable();
        }

        public virtual IKeyPurposeId GetIdKpOCSPSigning() {
            return ID_KP_OCSP_SIGNING;
        }
    }
}
