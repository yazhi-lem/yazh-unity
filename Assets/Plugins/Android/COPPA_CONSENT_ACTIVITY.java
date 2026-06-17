package org.yazhi.unity;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.util.Patterns;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.EditText;

import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;

/**
 * COPPA Consent Dialog — Android
 * KARUPPU | Rotation 25 Cycle 9 | Jun 18, 2026
 *
 * Implements US COPPA + India DPDP compliance for users under 13/18 respectively.
 * Collects parental consent before granting camera/AR access to minors.
 * Logs consent decisions to audit trail for regulatory review.
 */
public class CoppaConsentActivity extends Activity {
    private static final String TAG = "YazhiCoppa";
    private static final String PREFS_NAME = "yazhi_coppa_consent";
    private static final String KEY_CONSENT_GRANTED = "consent_granted";
    private static final String KEY_CONSENT_TIMESTAMP = "consent_timestamp";
    private static final String KEY_PARENT_EMAIL = "parent_email";
    private static final String AUDIT_LOG_FILE = "coppa_consent_audit.log";

    private CheckBox parentConfirmCheckbox;
    private EditText parentEmailEdit;
    private Button acceptButton;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        // Check if already consented
        SharedPreferences prefs = getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE);
        boolean alreadyConsented = prefs.getBoolean(KEY_CONSENT_GRANTED, false);

        if (alreadyConsented) {
            Log.d(TAG, "COPPA consent already granted — proceeding");
            setResult(RESULT_OK);
            finish();
            return;
        }

        // Show consent dialog
        showConsentDialog();
    }

    private void showConsentDialog() {
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        LayoutInflater inflater = getLayoutInflater();
        View dialogView = inflater.inflate(R.layout.coppa_consent_dialog, null);

        parentConfirmCheckbox = dialogView.findViewById(R.id.coppa_parent_confirm);
        parentEmailEdit = dialogView.findViewById(R.id.coppa_parent_email);
        Button declineButton = dialogView.findViewById(R.id.coppa_decline);
        acceptButton = dialogView.findViewById(R.id.coppa_accept);

        // Enable Accept only when checkbox is checked AND email is valid
        CompoundButton.OnCheckedChangeListener checkListener = (buttonView, isChecked) -> updateAcceptButtonState();
        parentConfirmCheckbox.setOnCheckedChangeListener(checkListener);
        parentEmailEdit.addTextChangedListener(new android.text.TextWatcher() {
            @Override public void beforeTextChanged(CharSequence s, int start, int count, int after) {}
            @Override public void onTextChanged(CharSequence s, int start, int before, int count) { updateAcceptButtonState(); }
            @Override public void afterTextChanged(android.text.Editable s) {}
        });

        declineButton.setOnClickListener(v -> {
            Log.w(TAG, "COPPA consent DECLINED — exiting");
            logConsentDecision(false, null);
            setResult(RESULT_CANCELED);
            finish();
        });

        acceptButton.setOnClickListener(v -> {
            String email = parentEmailEdit.getText().toString().trim();
            if (!isValidEmail(email)) {
                parentEmailEdit.setError(getString(R.string.coppa_email_invalid));
                return;
            }
            if (!parentConfirmCheckbox.isChecked()) {
                Log.w(TAG, "Accept clicked but parent not confirmed");
                return;
            }
            Log.d(TAG, "COPPA consent GRANTED — parent email: " + email);
            saveConsent(email);
            logConsentDecision(true, email);
            setResult(RESULT_OK);
            finish();
        });

        builder.setView(dialogView);
        builder.setCancelable(false); // Must make a choice
        AlertDialog dialog = builder.create();
        dialog.show();
    }

    private void updateAcceptButtonState() {
        boolean checked = parentConfirmCheckbox != null && parentConfirmCheckbox.isChecked();
        boolean emailValid = parentEmailEdit != null && isValidEmail(parentEmailEdit.getText().toString().trim());
        if (acceptButton != null) {
            acceptButton.setEnabled(checked && emailValid);
        }
    }

    private boolean isValidEmail(String email) {
        return !TextUtils.isEmpty(email) && Patterns.EMAIL_ADDRESS.matcher(email).matches();
    }

    private void saveConsent(String parentEmail) {
        SharedPreferences prefs = getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = prefs.edit();
        editor.putBoolean(KEY_CONSENT_GRANTED, true);
        editor.putString(KEY_CONSENT_TIMESTAMP, new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.US).format(new Date()));
        editor.putString(KEY_PARENT_EMAIL, parentEmail);
        editor.apply();
        Log.d(TAG, "Consent saved to SharedPreferences");
    }

    private void logConsentDecision(boolean granted, String parentEmail) {
        // Audit log for COPPA/DPDP compliance review
        String timestamp = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.US).format(new Date());
        String entry = String.format(Locale.US,
            "[COPPA-AUDIT] timestamp=%s platform=android granted=%s parent_email=%s locale=%s\n",
            timestamp, granted, parentEmail != null ? parentEmail : "N/A", Locale.getDefault());

        Log.d(TAG, entry.trim());

        // Append to file (for regulatory review)
        try {
            File logFile = new File(getFilesDir(), AUDIT_LOG_FILE);
            FileWriter writer = new FileWriter(logFile, true);
            writer.append(entry);
            writer.close();
        } catch (IOException e) {
            Log.e(TAG, "Failed to write COPPA audit log", e);
        }
    }

    /**
     * Public API: Check if COPPA consent has been granted
     */
    public static boolean hasConsent(Context context) {
        SharedPreferences prefs = context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE);
        return prefs.getBoolean(KEY_CONSENT_GRANTED, false);
    }

    /**
     * Public API: Revoke COPPA consent (for GDPR/DPDP right-to-erasure)
     */
    public static void revokeConsent(Context context) {
        SharedPreferences prefs = context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE);
        prefs.edit().clear().apply();
        Log.d(TAG, "COPPA consent REVOKED");
    }
}
