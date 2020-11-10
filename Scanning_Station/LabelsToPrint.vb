Module LabelsToPrint
    Public Function IP_Lab(SN As ArrayList, LabelScenario As String, x As String, y As String, COM3 As IO.Ports.SerialPort, COM6 As IO.Ports.SerialPort) As String
        Dim StrToPrint As String
        Select Case LabelScenario
            Case "Этикетки 45х8 и 39х19"
                StrToPrint = "
^XA~TA000~JSN^LT0^MNW^MTT^PON^PMN^LH" & x & "," & y & "^JMA^JUS^LRN^CI0^XZ
^XA
^MMT
^PW531
^LL0154
^LS0
^BY2,3,41^FT83,106^BCN,,N,N
^FD>:" & Mid(SN(1), 1, 2) & ">5" & Mid(SN(1), 3) & "^FS
^FT76,62^A0N,29,28^FH\^FDS/N:^FS
^FT129,62^A0N,29,28^FH\^FD" & SN(1) & "^FS
^PQ3,0,1,Y^XZ"
                COM6.Open()
                COM6.Write(StrToPrint) 'ответ в COM порт
                COM6.Close()

                StrToPrint = "
^XA~TA000~JSN^LT0^MNW^MTT^PON^PMN^LH" & x & "," & y & "^JMA^JUS^LRN^CI0^XZ
^XA
^MMT
^PW461
^LL0236
^LS0
^BY2,3,56^FT31,130^BCN,,N,N
^FD>:" & Mid(SN(1), 1, 2) & ">5" & Mid(SN(1), 3) & "^FS
^FT24,70^A0N,29,28^FH\^FDS/N:^FS
^FT77,70^A0N,29,28^FH\^FD" & SN(1) & "^FS
^BY2,3,41^FT31,209^BCN,,N,N
^FD>:" & SN(2) & "^FS
^FT24,165^A0N,29,28^FH\^FDMAC:^FS
^FT86,165^A0N,29,28^FH\^FD" & SN(3) & "^FS
^PQ1,0,1,Y^XZ"
                COM3.Open()
                COM3.Write(StrToPrint) 'ответ в COM порт
                COM3.Close()

            Case "Этикетка 44х21_Rus"
                StrToPrint = "
^XA~TA000~JSN^LT0^MNW^MTT^PON^PMN^LH" & x & "," & y & "^JMA^JUS^LRN^CI0^XZ
^XA
^MMT
^PW520
^LL0260
^LS0
^FO256,64^GFA,00128,00128,00004,:Z64:
eJxjYKAcnLf53ADC5OgFABpjBP0=:4D97
^FO0,32^GFA,01152,01152,00012,:Z64:
eJztkrENwyAQRQ9RUDICK2QDMlY6Z7SM4hFcprD8gxH8+3WkFJFM9Qr7PXSc2XW+P3fh1TFszvHtnHbnfDgXOFc8yQteZICBADAQAQYSwEAGGCgAA7XxDCyNZ6DhDISTN+oZSCfv1JOLfFPl30WcJ45W1487dD1Ef/xMH0Rvou+eOcLq+t4d+n6fOakkE4wy2SATN3mJFvAVqP5yLbCSs7x0kg2IshlBNsZkk+whfLP/Oh9ml7r4:918A
^FO0,32^GFA,01152,01152,00012,:Z64:
eJztkrERAjEMBOVx4NB04BYIydwWATOmNEqhBEKCHw4pOP1V8EOAog3ev/adzH5jTsIX4ZvwZ8eCnSvuyQ2P5I5n8sAreeKdvLAlAykoQAqqMwXNmYLuTMFwpmA6U7CcKXBMQTDkGwriLAXxTwrCRUHcgYK4GwXlcAFftuTFU5IYklCX5JokWiXpIg0YpJkljU1pckjDXZpvshFVNqXIBtkmfBU+23+Omy+esbC5:2713
^FO96,0^GFA,01920,01920,00020,:Z64:
eJzt0LFKBDEQBuB/CWyaJbZbhNtXmDKVz7K+gKS8SlcEr9GztfEdrrTMseB1PkPkeondFYtxEkWPsxIbi/xFIB+ZzDBASUnJbyIG9X2JgTqEQ7PnlxOEm+1V9eHENQe2XcEpCH+8Z44+TfjOzWXgourLjkK3nthm3JCN/wtn2uhxWVO6gCqnIVanrdZjzQZItgue7+HF6HYjsymQqHZsT6bJNmVTfIi7bLGmCGiQRg1xz2Y2UDQOaEEm2c3SaEq2HmBANtnthzXJLKhPtlC6tSMb1/Ygn3oslDJ+fOS31/Bskme+UvI5vk5zelNpp3G7y9uonP2x/79YScl/yjvJJ2M0:BE00
^FO96,64^GFA,01792,01792,00028,:Z64:
eJzt0D1qwzAUB/AnNGgR8ZpCwFdQ6VANwWeR8QU8diiRSoYsPkIP0SPIaPAScoJCFTx0VTcNoa78QZBLO5Yu/m+Pn6Sn9wCWLFnyb0nVrPRxIfXMuvionT/TRkfR/BqcoxrP24GNDdCFKImtEBkkGqx9RC5jTOvRUi0TJ0QHiQNbfxLfsbIORgAduS0oF7lRlIM1hxXSzJnJmCgg2JOmazgbXCF1taoseyuCcXg/4Gesbl0TmReF0pRBR1Cwu5fTZNIVxMtW6cSCHY0dB8NVCe2KD/f4ZPfstZ9BYRpsM/ZbB+v7ZcxfzXyzLXO63ycJP+gtr1VveB/mGw0+TnSbG/ogRDvMjprkpuVs2Kt869Jmn3ghdsPO4ILdbtwnwIbAr/kLW/JTvgAoH43n:977F
^FO96,32^GFA,02048,02048,00032,:Z64:
eJzt0TFLw0AUB/B3HDTLkVtvKPQrZDwXP8uVgqOmW8WlQUiWoGs3v4If4YWAXaSfQDQhg2tFkA6h8V3TBI3axUnMf3v3473j7gH06dOnz98M59+fq2OJbJ796PpZZvyAb1LhywOueOyxYE3OERGMmak7ZWbOTQHYeBLckst1Qp5vycdb96lgexfT2oWXI/h5aD1UzX3kGbtcAK+Utl445BNHtz6Q5Ir6lXpFOC+4de61LoTf9VFhfVO7Fh6Lyp2/IVxM2n5Vuy90448IZ1/ckIcu+VDvPLIe6daR5l9Zd/2HJZyOV/Z9q2HKj3Dv0yQekDvZ/RJOTDmqFqaUKX+pvbrOg9j5+OVs/nkFv627m+36v8g7gn1vDQ==:A0BD
^FO320,64^GFA,01536,01536,00024,:Z64:
eJzt0T8KwjAUx/EnEbq1FwjJMazwbK7kZCsEFTp4HjfBDhXBbnoDiXSXFAcdpJo2i39R936H3/DhLSEATU1N/9eud/LitBry6liN89UjXyyXpevBNWHQu2qyi+oLX3ZX+w13axfGS+soh3m+5eyjI6UQOBOirFPZz/Mpp4AMAY2n1tlBn8LpIgF0/V/cPagwJIs5oPfoazXokJlxp3/vXqZGghRnY9q6lPXLszQIWoVGEStE57hKx5f3LqzHKbKW0sgrJ8Y5/v5pTU/dAAF6bsw=:6D1F
^FO352,32^GFA,01280,01280,00020,:Z64:
eJzt0zEOwyAMBVAjBkaOwE3K0WDLtXKUHKFjBoRrQ6Vg1x0jdShLpKfIGPwB+C9a8QDwJ0BqGbc+rZAlaQ7JirRA5lBaJPPKElkYVh9p24dlsqiskCVlSJaljS0zl17t3d5iHulTnsoq1TyEBeR9dmmda1Zp7ZuBsDiMf1zstM1P47u6zVy3+pNmncM8L9fSZtyfdc/WPObcGsiZN84HfORAZ6MYGbJyla5Mkl05dco4z6BsNJiljfcRlfE7AmU/vV4zzwDx:0A36
^FO256,76^GB25,0,2^FS
^FO7,236^GB490,0,4^FS
^FO5,124^GB490,0,4^FS
^FO81,42^GB0,75,10^FS
^BY2,3,23^FT15,184^BCN,,N,N
^FD>:" & Mid(SN(1), 1, 2) & ">5" & Mid(SN(1), 3) & "^FS
^FT14,156^A0N,21,21^FH\^FDS/N:^FS
^FT60,156^A0N,21,21^FH\^FD" & SN(1) & "^FS
^BY2,3,23^FT15,229^BCN,,N,N
^FD>:" & SN(2) & "^FS
^FT15,205^A0N,21,21^FH\^FDMAC:^FS
^FT67,205^A0N,21,21^FH\^FD" & SN(3) & "^FS
^PQ1,0,1,Y^XZ
"
                COM3.Open()
                COM3.Write(StrToPrint) 'ответ в COM порт
                COM3.Close()
        End Select
        Return StrToPrint
    End Function



End Module
