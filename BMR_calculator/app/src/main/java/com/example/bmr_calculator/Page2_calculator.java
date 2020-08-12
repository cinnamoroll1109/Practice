package com.example.bmr_calculator;

import androidx.appcompat.app.AppCompatActivity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.*;
import android.util.Log;

public class Page2_calculator extends AppCompatActivity {
    private Button btn_send;
    private Button btn_cle;
    private RadioGroup rg_sex;
    private RadioButton rb_woman;
    private RadioButton rb_man;
    private EditText et_name;
    private EditText et_age;
    private EditText et_height;
    private EditText et_weight;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_page2_calculator);

        init_objects();

        View.OnClickListener btn_sendOnClick = new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (!("".equals(et_name.getText().toString())
                        || "".equals(et_age.getText().toString())
                        || "".equals((et_height.getText().toString()))
                        || "".equals(et_weight.getText().toString())))
                {
                    Bundle bundle = new Bundle();
                    bundle.putString("name", et_name.getText().toString());
                    //String sex = "";
                    if (rb_man.isChecked()){
                        //sex = "man";
                        bundle.putString("sex", "man");
                    }
                    else{
                        //sex = "woman";
                        bundle.putString("sex", "woman");
                    }
                    bundle.putString("age", et_age.getText().toString());
                    bundle.putString("height", et_height.getText().toString());
                    bundle.putString("weight", et_weight.getText().toString());
                    bundle.putString("insert_or_update", "insert");

                    Intent it = new Intent();
                    it.putExtras(bundle);
                    it.setClass(Page2_calculator.this, Page3_output.class);
                    startActivity(it);
                }
            }
        };
        btn_send.setOnClickListener(btn_sendOnClick);

        View.OnClickListener btn_cleOnClick = new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                et_name.setText("");
                et_age.setText("");
                et_height.setText("");
                et_weight.setText("");
            }
        };
        btn_cle.setOnClickListener(btn_cleOnClick);
    }

    private void init_objects(){
        btn_send = findViewById(R.id.p2_btnSend);
        btn_cle = findViewById(R.id.p2_btnClear);
        rg_sex = findViewById(R.id.p2_radio_group1);
        rb_woman = findViewById(R.id.p2_rbWoman);
        rb_man = findViewById(R.id.p2_rbMan);
        et_name = findViewById(R.id.p2_etName);
        et_age = findViewById(R.id.p2_etAge);
        et_height = findViewById(R.id.p2_etHeight);
        et_weight = findViewById(R.id.p2_etWeight);
    }

}