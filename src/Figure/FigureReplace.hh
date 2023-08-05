#ifndef FIGUREREPLACE_HEADER
#define FIGUREREPLACE_HEADER

#include <string>

#include <il2cpp_api.hh>

#include "Hook/HookTemplate.hh"

class FigureData {
public:
    class CharaAssets {
    public:
        MyArray<unsigned char>* a_ = nullptr;
        MyArray<unsigned char>* b_ = nullptr;
        MyArray<unsigned char>* c_ = nullptr;
    };

    CharaAssets narrow_;
    CharaAssets chara_;
    CharaAssets status_;

    FigureData(){};
    FigureData(MyArray<unsigned char>* narrow_a, MyArray<unsigned char>* narrow_b,
               MyArray<unsigned char>* chara_a, MyArray<unsigned char>* chara_b) {
        narrow_.a_ = narrow_a;
        narrow_.b_ = narrow_b;
        chara_.a_ = chara_a;
        chara_.b_ = chara_b;
    };
};

class FigureReplace : public HookTemplate {
public:
    virtual void Start() override final;
};

#endif // FigureReplace.hh